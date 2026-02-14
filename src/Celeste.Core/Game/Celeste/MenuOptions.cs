using System;
using System.Globalization;
using Celeste.Core.Platform.Interop;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Monocle;

namespace Celeste;

public static class MenuOptions
{
	private static TextMenu menu;

	private static bool inGame;

	private static TextMenu.Item crouchDashMode;

	private static TextMenu.Item window;

	private static TextMenu.Item viewport;

	private static EventInstance snapshot;

	private static readonly string[] RuntimeTargetFpsValues = new string[5] { "30", "60", "90", "120", "Uncapped" };

	private static readonly float[] RuntimeOverlayFontScaleValues = new float[8] { 0.5f, 0.75f, 1f, 1.25f, 1.5f, 1.75f, 2f, 2.5f };

	private static readonly int[] RuntimeOverlayUpdateMsValues = new int[6] { 100, 250, 500, 1000, 2000, 5000 };

	private static readonly int[] RuntimeOverlayPaddingValues = new int[7] { 0, 4, 8, 12, 16, 24, 32 };

	private static readonly int[] RuntimeTouchOpacityValues = new int[8] { 35, 45, 55, 65, 75, 85, 95, 100 };

	private static readonly float[] RuntimeTouchScaleValues = new float[7] { 0.7f, 0.85f, 1f, 1.15f, 1.3f, 1.45f, 1.6f };

	private static readonly float[] RuntimeTouchDeadzoneValues = new float[8] { 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.45f, 0.6f };

	private static readonly float[] RuntimeTouchLeftXValues = new float[9] { 0.1f, 0.13f, 0.16f, 0.18f, 0.2f, 0.23f, 0.26f, 0.3f, 0.35f };

	private static readonly float[] RuntimeTouchLeftYValues = new float[9] { 0.56f, 0.62f, 0.68f, 0.72f, 0.76f, 0.8f, 0.84f, 0.88f, 0.92f };

	private static readonly float[] RuntimeTouchTopYValues = new float[9] { 0.06f, 0.08f, 0.1f, 0.12f, 0.13f, 0.15f, 0.18f, 0.22f, 0.26f };

	private static readonly float[] RuntimeTouchActionXValues = new float[9] { 0.58f, 0.63f, 0.68f, 0.73f, 0.78f, 0.82f, 0.86f, 0.9f, 0.94f };

	private static readonly float[] RuntimeTouchActionYValues = new float[9] { 0.56f, 0.62f, 0.68f, 0.72f, 0.74f, 0.78f, 0.82f, 0.86f, 0.9f };

	private static readonly float[] RuntimeTouchStickRadiusValues = new float[7] { 0.08f, 0.1f, 0.12f, 0.14f, 0.16f, 0.18f, 0.2f };

	private static readonly float[] RuntimeTouchButtonRadiusValues = new float[8] { 0.05f, 0.06f, 0.07f, 0.08f, 0.09f, 0.11f, 0.13f, 0.14f };

	private static readonly float[] RuntimeTouchSpacingValues = new float[7] { 1.05f, 1.2f, 1.35f, 1.5f, 1.7f, 1.85f, 2f };

	[Tracked(false)]
	private sealed class TouchSettingsUI : TextMenu
	{
		private bool open = true;

		public TouchSettingsUI()
		{
			base.Tag = (int)Tags.HUD | (int)Tags.PauseUpdate;
			Alpha = 0f;
			MinWidth = 760f;
			Add(new Header(RuntimeText("runtime_touch")));
			if (CelesteRuntimeConfigBridge.TryGetSnapshot(out RuntimeUiConfigSnapshot snapshot))
			{
				Add(new SubHeader(RuntimeText("runtime_touch_general")));
				Add(new OnOff(RuntimeText("runtime_touch_enabled"), snapshot.TouchEnabled).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchEnabled = on;
					});
				}));
				Add(new OnOff(RuntimeText("runtime_touch_gameplay_only"), snapshot.TouchGameplayOnly).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchGameplayOnly = on;
					});
				}));
				Add(new OnOff(RuntimeText("runtime_touch_auto_disable"), snapshot.TouchAutoDisableOnExternalInput).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchAutoDisableOnExternalInput = on;
					});
				}));
				Add(new OnOff(RuntimeText("runtime_touch_menu_tap"), snapshot.TouchTapMenuNavigation).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchTapMenuNavigation = on;
					});
				}));
				Add(new Button(RuntimeText("runtime_touch_editor_open")).Pressed(delegate
				{
					Focused = false;
					TouchLayoutEditorUI touchLayoutEditorUI = new TouchLayoutEditorUI();
					touchLayoutEditorUI.OnClose = delegate
					{
						Focused = true;
					};
					Engine.Scene.Add(touchLayoutEditorUI);
					Engine.Scene.OnEndOfFrame += delegate
					{
						Engine.Scene.Entities.UpdateLists();
					};
				}));
				Add(new SubHeader(RuntimeText("runtime_touch_layout")));
				Add(new OnOff(RuntimeText("runtime_touch_shoulders"), snapshot.TouchEnableShoulders).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchEnableShoulders = on;
					});
				}));
				Add(new OnOff(RuntimeText("runtime_touch_start_select"), snapshot.TouchEnableStartSelect).Change(delegate(bool on)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchEnableStartSelect = on;
					});
				}));
				int touchShoulderYIndex = FindNearestIndex(RuntimeTouchTopYValues, snapshot.TouchShoulderY);
				Add(new Slider(RuntimeText("runtime_touch_shoulder_y"), (int i) => (RuntimeTouchTopYValues[Math.Clamp(i, 0, RuntimeTouchTopYValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchTopYValues.Length - 1, touchShoulderYIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchShoulderY = RuntimeTouchTopYValues[Math.Clamp(i, 0, RuntimeTouchTopYValues.Length - 1)];
					});
				}));
				int touchStartYIndex = FindNearestIndex(RuntimeTouchTopYValues, snapshot.TouchStartSelectY);
				Add(new Slider(RuntimeText("runtime_touch_start_select_y"), (int i) => (RuntimeTouchTopYValues[Math.Clamp(i, 0, RuntimeTouchTopYValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchTopYValues.Length - 1, touchStartYIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchStartSelectY = RuntimeTouchTopYValues[Math.Clamp(i, 0, RuntimeTouchTopYValues.Length - 1)];
					});
				}));
				int touchOpacityIndex = FindNearestIndex(RuntimeTouchOpacityValues, (int)Math.Round(snapshot.TouchOpacity * 100f));
				Add(new Slider(RuntimeText("runtime_touch_opacity"), (int i) => RuntimeTouchOpacityValues[Math.Clamp(i, 0, RuntimeTouchOpacityValues.Length - 1)] + "%", 0, RuntimeTouchOpacityValues.Length - 1, touchOpacityIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchOpacity = (float)RuntimeTouchOpacityValues[Math.Clamp(i, 0, RuntimeTouchOpacityValues.Length - 1)] / 100f;
					});
				}));
				int touchScaleIndex = FindNearestIndex(RuntimeTouchScaleValues, snapshot.TouchScale);
				Add(new Slider(RuntimeText("runtime_touch_scale"), (int i) => (RuntimeTouchScaleValues[Math.Clamp(i, 0, RuntimeTouchScaleValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchScaleValues.Length - 1, touchScaleIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchScale = RuntimeTouchScaleValues[Math.Clamp(i, 0, RuntimeTouchScaleValues.Length - 1)];
					});
				}));
				Add(new SubHeader(RuntimeText("runtime_touch_movement")));
				int touchDeadzoneIndex = FindNearestIndex(RuntimeTouchDeadzoneValues, snapshot.TouchLeftStickDeadzone);
				Add(new Slider(RuntimeText("runtime_touch_deadzone"), (int i) => (RuntimeTouchDeadzoneValues[Math.Clamp(i, 0, RuntimeTouchDeadzoneValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchDeadzoneValues.Length - 1, touchDeadzoneIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchLeftStickDeadzone = RuntimeTouchDeadzoneValues[Math.Clamp(i, 0, RuntimeTouchDeadzoneValues.Length - 1)];
					});
				}));
				int touchStickRadiusIndex = FindNearestIndex(RuntimeTouchStickRadiusValues, snapshot.TouchLeftStickRadius);
				Add(new Slider(RuntimeText("runtime_touch_stick_radius"), (int i) => (RuntimeTouchStickRadiusValues[Math.Clamp(i, 0, RuntimeTouchStickRadiusValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchStickRadiusValues.Length - 1, touchStickRadiusIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchLeftStickRadius = RuntimeTouchStickRadiusValues[Math.Clamp(i, 0, RuntimeTouchStickRadiusValues.Length - 1)];
					});
				}));
				int touchLeftXIndex = FindNearestIndex(RuntimeTouchLeftXValues, snapshot.TouchLeftStickX);
				Add(new Slider(RuntimeText("runtime_touch_left_x"), (int i) => (RuntimeTouchLeftXValues[Math.Clamp(i, 0, RuntimeTouchLeftXValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchLeftXValues.Length - 1, touchLeftXIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchLeftStickX = RuntimeTouchLeftXValues[Math.Clamp(i, 0, RuntimeTouchLeftXValues.Length - 1)];
					});
				}));
				int touchLeftYIndex = FindNearestIndex(RuntimeTouchLeftYValues, snapshot.TouchLeftStickY);
				Add(new Slider(RuntimeText("runtime_touch_left_y"), (int i) => (RuntimeTouchLeftYValues[Math.Clamp(i, 0, RuntimeTouchLeftYValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchLeftYValues.Length - 1, touchLeftYIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchLeftStickY = RuntimeTouchLeftYValues[Math.Clamp(i, 0, RuntimeTouchLeftYValues.Length - 1)];
					});
				}));
				Add(new SubHeader(RuntimeText("runtime_touch_actions")));
				int touchButtonRadiusIndex = FindNearestIndex(RuntimeTouchButtonRadiusValues, snapshot.TouchButtonRadius);
				Add(new Slider(RuntimeText("runtime_touch_button_radius"), (int i) => (RuntimeTouchButtonRadiusValues[Math.Clamp(i, 0, RuntimeTouchButtonRadiusValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchButtonRadiusValues.Length - 1, touchButtonRadiusIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchButtonRadius = RuntimeTouchButtonRadiusValues[Math.Clamp(i, 0, RuntimeTouchButtonRadiusValues.Length - 1)];
					});
				}));
				int touchSpacingIndex = FindNearestIndex(RuntimeTouchSpacingValues, snapshot.TouchActionSpacing);
				Add(new Slider(RuntimeText("runtime_touch_spacing"), (int i) => "x" + RuntimeTouchSpacingValues[Math.Clamp(i, 0, RuntimeTouchSpacingValues.Length - 1)].ToString("0.##", CultureInfo.InvariantCulture), 0, RuntimeTouchSpacingValues.Length - 1, touchSpacingIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchActionSpacing = RuntimeTouchSpacingValues[Math.Clamp(i, 0, RuntimeTouchSpacingValues.Length - 1)];
					});
				}));
				int touchActionXIndex = FindNearestIndex(RuntimeTouchActionXValues, snapshot.TouchActionX);
				Add(new Slider(RuntimeText("runtime_touch_action_x"), (int i) => (RuntimeTouchActionXValues[Math.Clamp(i, 0, RuntimeTouchActionXValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchActionXValues.Length - 1, touchActionXIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchActionX = RuntimeTouchActionXValues[Math.Clamp(i, 0, RuntimeTouchActionXValues.Length - 1)];
					});
				}));
				int touchActionYIndex = FindNearestIndex(RuntimeTouchActionYValues, snapshot.TouchActionY);
				Add(new Slider(RuntimeText("runtime_touch_action_y"), (int i) => (RuntimeTouchActionYValues[Math.Clamp(i, 0, RuntimeTouchActionYValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeTouchActionYValues.Length - 1, touchActionYIndex).Change(delegate(int i)
				{
					ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
					{
						update.TouchActionY = RuntimeTouchActionYValues[Math.Clamp(i, 0, RuntimeTouchActionYValues.Length - 1)];
					});
				}));
			}
			OnESC = (OnPause = (OnCancel = delegate
			{
				open = false;
				Focused = false;
			}));
			Position.Y = base.ScrollTargetY;
		}

		public override void Update()
		{
			if (Alpha > 0f)
			{
				base.Update();
			}
			if (!open && Alpha <= 0f)
			{
				Close();
			}
			Alpha = Calc.Approach(Alpha, open ? 1f : 0f, Engine.DeltaTime * 8f);
		}

		public override void Render()
		{
			if (Alpha > 0f)
			{
				Draw.Rect(-10f, -10f, 1940f, 1100f, Color.Black * Ease.CubeOut(Alpha));
				base.Render();
			}
		}
	}

	[Tracked(false)]
	private sealed class TouchLayoutEditorUI : Entity
	{
		private enum DragTarget
		{
			None,
			LeftStick,
			ShoulderCluster,
			StartSelectCluster,
			ActionCluster
		}

		private bool open = true;

		private float alpha;

		private DragTarget dragTarget;

		private bool pinchActive;

		private float pinchDistance;

		private float leftX = 0.18f;

		private float leftY = 0.76f;

		private float actionX = 0.82f;

		private float actionY = 0.74f;

		private float shoulderY = 0.13f;

		private float startSelectY = 0.12f;

		private float scale = 1f;

		private float leftStickRadius = 0.12f;

		private float buttonRadius = 0.07f;

		private float actionSpacing = 1.35f;

		private Rectangle resetRect;

		private Rectangle doneRect;

		private Rectangle snapRect;

		private Rectangle precisionRect;

		private bool snapToAnchors = true;

		private bool precisionMode;

		private bool pendingLayoutUpdate;

		private bool pendingScaleUpdate;

		private float pendingApplyTimer;

		private Vector2 dragStartPointer;

		private float dragStartLeftX;

		private float dragStartLeftY;

		private float dragStartActionX;

		private float dragStartActionY;

		private float dragStartShoulderY;

		private float dragStartStartSelectY;

		public Action OnClose;

		public TouchLayoutEditorUI()
		{
			base.Tag = (int)Tags.HUD | (int)Tags.PauseUpdate;
			if (CelesteRuntimeConfigBridge.TryGetSnapshot(out RuntimeUiConfigSnapshot snapshot))
			{
				leftX = snapshot.TouchLeftStickX;
				leftY = snapshot.TouchLeftStickY;
				actionX = snapshot.TouchActionX;
				actionY = snapshot.TouchActionY;
				shoulderY = snapshot.TouchShoulderY;
				startSelectY = snapshot.TouchStartSelectY;
				scale = snapshot.TouchScale;
				leftStickRadius = snapshot.TouchLeftStickRadius;
				buttonRadius = snapshot.TouchButtonRadius;
				actionSpacing = snapshot.TouchActionSpacing;
			}

			UpdateButtonsLayout();
		}

		public override void Update()
		{
			base.Update();
			alpha = Calc.Approach(alpha, open ? 1f : 0f, Engine.RawDeltaTime * 8f);
			if (!open && alpha <= 0f)
			{
				FlushLayoutUpdate(force: true);
				RemoveSelf();
				if (OnClose != null)
				{
					OnClose();
				}
				return;
			}

			if (Input.MenuCancel.Pressed || Input.ESC.Pressed || Input.Pause.Pressed)
			{
				dragTarget = DragTarget.None;
				open = false;
				return;
			}

			UpdateButtonsLayout();
			UpdatePinchScale();
			UpdatePointerEditing();
			FlushLayoutUpdate(force: dragTarget == DragTarget.None && !pinchActive);
		}

		public override void Render()
		{
			if (alpha <= 0f)
			{
				return;
			}

			float eased = Ease.CubeOut(alpha);
			Draw.Rect(-10f, -10f, 1940f, 1100f, Color.Black * 0.84f * eased);

			float width = Engine.Width;
			float height = Engine.Height;
			ResolvePreviewLayout(width, height, out var leftCenter, out var shoulderCenter, out var startSelectCenter, out var actionCenter, out var aCenter, out var bCenter, out var xCenter, out var yCenter, out float leftRadiusPx, out float buttonRadiusPx);

			Draw.Rect(width * 0.06f, height * 0.4f, width * (0.45f - 0.06f), height * (0.95f - 0.4f), new Color(45, 68, 96) * 0.23f * eased);
			Draw.HollowRect(width * 0.06f, height * 0.4f, width * (0.45f - 0.06f), height * (0.95f - 0.4f), new Color(88, 130, 178) * 0.7f * eased);
			Draw.Rect(width * 0.52f, height * 0.4f, width * (0.95f - 0.52f), height * (0.95f - 0.4f), new Color(72, 58, 92) * 0.23f * eased);
			Draw.HollowRect(width * 0.52f, height * 0.4f, width * (0.95f - 0.52f), height * (0.95f - 0.4f), new Color(160, 116, 194) * 0.7f * eased);

			Draw.Circle(leftCenter, leftRadiusPx * 1.05f, new Color(43, 63, 92) * 0.72f * eased, 42);
			Draw.Circle(leftCenter, leftRadiusPx * 0.5f, new Color(178, 208, 235) * 0.75f * eased, 32);
			float shoulderYPos = shoulderCenter.Y;
			Draw.Circle(new Vector2(width * 0.23f, shoulderYPos), buttonRadiusPx * 0.85f, new Color(132, 144, 166) * 0.9f * eased, 28);
			Draw.Circle(new Vector2(width * 0.77f, shoulderYPos), buttonRadiusPx * 0.85f, new Color(132, 144, 166) * 0.9f * eased, 28);
			Draw.Circle(new Vector2(width * 0.1f, shoulderYPos * 0.9f), buttonRadiusPx * 0.75f, new Color(106, 120, 142) * 0.9f * eased, 28);
			Draw.Circle(new Vector2(width * 0.9f, shoulderYPos * 0.9f), buttonRadiusPx * 0.75f, new Color(106, 120, 142) * 0.9f * eased, 28);
			Draw.Circle(new Vector2(width * 0.43f, startSelectCenter.Y), buttonRadiusPx * 0.68f, new Color(88, 102, 128) * 0.9f * eased, 28);
			Draw.Circle(new Vector2(width * 0.57f, startSelectCenter.Y), buttonRadiusPx * 0.68f, new Color(88, 102, 128) * 0.9f * eased, 28);
			Draw.Circle(actionCenter, buttonRadiusPx * 2.15f, new Color(58, 48, 74) * 0.35f * eased, 48);
			Draw.Circle(aCenter, buttonRadiusPx, new Color(58, 182, 102) * 0.9f * eased, 30);
			Draw.Circle(bCenter, buttonRadiusPx, new Color(217, 86, 89) * 0.9f * eased, 30);
			Draw.Circle(xCenter, buttonRadiusPx, new Color(79, 153, 243) * 0.9f * eased, 30);

			Color handleColor = dragTarget switch
			{
				DragTarget.LeftStick => new Color(123, 202, 255),
				DragTarget.ShoulderCluster => new Color(255, 231, 127),
				DragTarget.StartSelectCluster => new Color(255, 164, 212),
				DragTarget.ActionCluster => new Color(255, 184, 115),
				_ => new Color(240, 240, 240)
			};
			Draw.Circle(leftCenter, leftRadiusPx * 1.2f, handleColor * 0.85f * eased, 3f, 40);
			Draw.Circle(shoulderCenter, buttonRadiusPx * 1.75f, handleColor * 0.85f * eased, 3f, 40);
			Draw.Circle(startSelectCenter, buttonRadiusPx * 1.52f, handleColor * 0.85f * eased, 3f, 40);
			Draw.Circle(actionCenter, buttonRadiusPx * 2.45f, handleColor * 0.85f * eased, 3f, 40);
			DrawHandleTag(leftCenter, "LS", dragTarget == DragTarget.LeftStick, eased, new Vector2(0f, -leftRadiusPx * 1.45f));
			DrawHandleTag(shoulderCenter, "SH", dragTarget == DragTarget.ShoulderCluster, eased, new Vector2(0f, -buttonRadiusPx * 1.9f));
			DrawHandleTag(startSelectCenter, "ST", dragTarget == DragTarget.StartSelectCluster, eased, new Vector2(0f, -buttonRadiusPx * 1.85f));
			DrawHandleTag(actionCenter, "ABX", dragTarget == DragTarget.ActionCluster, eased, new Vector2(0f, -buttonRadiusPx * 2.65f));

			ActiveFont.DrawOutline(RuntimeText("runtime_touch_editor_title"), new Vector2(width * 0.5f, 74f), new Vector2(0.5f, 0.5f), Vector2.One * 0.9f, Color.White * eased, 2f, Color.Black * eased);
			ActiveFont.DrawOutline(RuntimeText("runtime_touch_editor_hint_drag"), new Vector2(width * 0.5f, 118f), new Vector2(0.5f, 0.5f), Vector2.One * 0.55f, new Color(212, 224, 238) * eased, 2f, Color.Black * eased);
			ActiveFont.DrawOutline(RuntimeText("runtime_touch_editor_hint_pinch"), new Vector2(width * 0.5f, 146f), new Vector2(0.5f, 0.5f), Vector2.One * 0.55f, new Color(212, 224, 238) * eased, 2f, Color.Black * eased);
			ActiveFont.DrawOutline(BuildStatusLine(), new Vector2(width * 0.5f, 176f), new Vector2(0.5f, 0.5f), Vector2.One * 0.38f, new Color(228, 236, 246) * eased, 2f, Color.Black * eased);

			DrawEditorButton(snapRect, BuildModeButtonLabel(RuntimeText("runtime_touch_editor_snap"), snapToAnchors), snapRect.Contains(MInput.Mouse.X, MInput.Mouse.Y), snapToAnchors, eased);
			DrawEditorButton(precisionRect, BuildModeButtonLabel(RuntimeText("runtime_touch_editor_precision"), precisionMode), precisionRect.Contains(MInput.Mouse.X, MInput.Mouse.Y), precisionMode, eased);
			DrawEditorButton(resetRect, RuntimeText("runtime_touch_editor_reset"), resetRect.Contains(MInput.Mouse.X, MInput.Mouse.Y), false, eased);
			DrawEditorButton(doneRect, RuntimeText("runtime_touch_editor_done"), doneRect.Contains(MInput.Mouse.X, MInput.Mouse.Y), false, eased);
		}

		private void DrawEditorButton(Rectangle rect, string label, bool hovered, bool active, float eased)
		{
			Color fill = active ? new Color(48, 106, 92) : (hovered ? new Color(62, 93, 126) : new Color(42, 62, 86));
			Color border = active ? new Color(132, 232, 188) : new Color(164, 200, 232);
			Draw.Rect(rect, fill * 0.88f * eased);
			Draw.HollowRect(rect, border * 0.86f * eased);
			ActiveFont.DrawOutline(label, new Vector2(rect.Center.X, rect.Center.Y), new Vector2(0.5f, 0.5f), Vector2.One * 0.58f, Color.White * eased, 2f, Color.Black * eased);
		}

		private static string BuildModeButtonLabel(string title, bool enabled)
		{
			return title + ": " + (enabled ? RuntimeText("runtime_value_on") : RuntimeText("runtime_value_off"));
		}

		private void DrawHandleTag(Vector2 center, string label, bool selected, float eased, Vector2 offset)
		{
			Color color = selected ? Color.White : new Color(212, 224, 238);
			ActiveFont.DrawOutline(label, center + offset, new Vector2(0.5f, 0.5f), Vector2.One * 0.42f, color * eased, 2f, Color.Black * eased);
		}

		private string BuildStatusLine()
		{
			return "LS " + ToPercent(leftX) + "," + ToPercent(leftY)
				+ "  TOP " + ToPercent(shoulderY) + "/" + ToPercent(startSelectY)
				+ "  ABX " + ToPercent(actionX) + "," + ToPercent(actionY)
				+ "  SCALE " + ToPercent(scale)
				+ "  SNAP " + (snapToAnchors ? RuntimeText("runtime_value_on") : RuntimeText("runtime_value_off"))
				+ "  PRECISE " + (precisionMode ? RuntimeText("runtime_value_on") : RuntimeText("runtime_value_off"));
		}

		private static string ToPercent(float value)
		{
			int percent = (int)Math.Round(value * 100f);
			return percent.ToString(CultureInfo.InvariantCulture) + "%";
		}

		private void UpdateButtonsLayout()
		{
			int buttonWidth = 280;
			int buttonHeight = 62;
			int spacing = 36;
			int top = (int)(Engine.Height - 102f);
			int left = (int)(Engine.Width * 0.5f - buttonWidth - spacing * 0.5f);
			int modeTop = top - buttonHeight - 16;
			snapRect = new Rectangle(left, modeTop, buttonWidth, buttonHeight);
			precisionRect = new Rectangle(left + buttonWidth + spacing, modeTop, buttonWidth, buttonHeight);
			resetRect = new Rectangle(left, top, buttonWidth, buttonHeight);
			doneRect = new Rectangle(left + buttonWidth + spacing, top, buttonWidth, buttonHeight);
		}

		private void UpdatePinchScale()
		{
			if (!TryGetPinchDistance(TouchPanel.GetState(), out float distance))
			{
				pinchActive = false;
				pinchDistance = 0f;
				return;
			}

			if (!pinchActive)
			{
				pinchActive = true;
				pinchDistance = distance;
				return;
			}

			float delta = distance - pinchDistance;
			pinchDistance = distance;
			float minDimension = Math.Max(1f, MathF.Min(Engine.Width, Engine.Height));
			float sensitivity = precisionMode ? 1.1f : 2f;
			float nextScale = Math.Clamp(scale + delta / minDimension * sensitivity, 0.65f, 1.8f);
			if (Math.Abs(nextScale - scale) <= 0.0005f)
			{
				return;
			}

			scale = nextScale;
			QueueLayoutUpdate(includeScale: true);
		}

		private void UpdatePointerEditing()
		{
			Vector2 pointer = new Vector2(MInput.Mouse.X, MInput.Mouse.Y);
			if (MInput.Mouse.PressedLeftButton)
			{
				if (snapRect.Contains((int)pointer.X, (int)pointer.Y))
				{
					snapToAnchors = !snapToAnchors;
					return;
				}

				if (precisionRect.Contains((int)pointer.X, (int)pointer.Y))
				{
					precisionMode = !precisionMode;
					return;
				}

				if (resetRect.Contains((int)pointer.X, (int)pointer.Y))
				{
					ResetLayoutToDefaults();
					dragTarget = DragTarget.None;
					return;
				}

				if (doneRect.Contains((int)pointer.X, (int)pointer.Y))
				{
					dragTarget = DragTarget.None;
					FlushLayoutUpdate(force: true);
					open = false;
					return;
				}

				if (pinchActive)
				{
					dragTarget = DragTarget.None;
					return;
				}

				ResolvePreviewLayout(Engine.Width, Engine.Height, out var leftCenter, out var shoulderCenter, out var startSelectCenter, out var actionCenter, out _, out _, out _, out _, out float leftRadiusPx, out float buttonRadiusPx);
				float leftGrabRadius = leftRadiusPx * 1.35f;
				float shoulderGrabRadius = buttonRadiusPx * 1.8f;
				float startSelectGrabRadius = buttonRadiusPx * 1.6f;
				float actionGrabRadius = buttonRadiusPx * 2.6f;
				if ((pointer - leftCenter).LengthSquared() <= leftGrabRadius * leftGrabRadius)
				{
					dragTarget = DragTarget.LeftStick;
				}
				else if ((pointer - shoulderCenter).LengthSquared() <= shoulderGrabRadius * shoulderGrabRadius)
				{
					dragTarget = DragTarget.ShoulderCluster;
				}
				else if ((pointer - startSelectCenter).LengthSquared() <= startSelectGrabRadius * startSelectGrabRadius)
				{
					dragTarget = DragTarget.StartSelectCluster;
				}
				else if ((pointer - actionCenter).LengthSquared() <= actionGrabRadius * actionGrabRadius)
				{
					dragTarget = DragTarget.ActionCluster;
				}
				else
				{
					dragTarget = DragTarget.None;
				}

				if (dragTarget != DragTarget.None)
				{
					CaptureDragOrigin(pointer);
				}
			}

			if (MInput.Mouse.ReleasedLeftButton)
			{
				dragTarget = DragTarget.None;
				FlushLayoutUpdate(force: true);
				return;
			}

			if (!MInput.Mouse.CheckLeftButton || dragTarget == DragTarget.None || pinchActive)
			{
				return;
			}

			bool changed = false;
			float dx = (pointer.X - dragStartPointer.X) / Math.Max(1f, Engine.Width);
			float dy = (pointer.Y - dragStartPointer.Y) / Math.Max(1f, Engine.Height);
			float dragFactor = precisionMode ? 0.22f : 1f;
			dx *= dragFactor;
			dy *= dragFactor;
			if (dragTarget == DragTarget.LeftStick)
			{
				changed |= SetClamped(ref leftX, dragStartLeftX + dx, 0.06f, 0.45f);
				changed |= SetClamped(ref leftY, dragStartLeftY + dy, 0.4f, 0.95f);
			}
			else if (dragTarget == DragTarget.ShoulderCluster)
			{
				changed |= SetClamped(ref shoulderY, dragStartShoulderY + dy, 0.06f, 0.3f);
			}
			else if (dragTarget == DragTarget.StartSelectCluster)
			{
				changed |= SetClamped(ref startSelectY, dragStartStartSelectY + dy, 0.06f, 0.3f);
			}
			else
			{
				changed |= SetClamped(ref actionX, dragStartActionX + dx, 0.52f, 0.95f);
				changed |= SetClamped(ref actionY, dragStartActionY + dy, 0.4f, 0.95f);
			}

			if (changed)
			{
				ApplyAnchorSnap(dragTarget);
				QueueLayoutUpdate(includeScale: false);
			}
		}

		private void CaptureDragOrigin(Vector2 pointer)
		{
			dragStartPointer = pointer;
			dragStartLeftX = leftX;
			dragStartLeftY = leftY;
			dragStartActionX = actionX;
			dragStartActionY = actionY;
			dragStartShoulderY = shoulderY;
			dragStartStartSelectY = startSelectY;
		}

		private void ApplyAnchorSnap(DragTarget target)
		{
			if (!snapToAnchors)
			{
				return;
			}

			float snapThreshold = precisionMode ? 0.006f : 0.012f;
			switch (target)
			{
			case DragTarget.LeftStick:
				leftX = SnapIfNear(leftX, 0.18f, snapThreshold);
				leftY = SnapIfNear(leftY, 0.76f, snapThreshold);
				break;
			case DragTarget.ShoulderCluster:
				shoulderY = SnapIfNear(shoulderY, 0.13f, snapThreshold);
				break;
			case DragTarget.StartSelectCluster:
				startSelectY = SnapIfNear(startSelectY, 0.12f, snapThreshold);
				break;
			case DragTarget.ActionCluster:
				actionX = SnapIfNear(actionX, 0.82f, snapThreshold);
				actionY = SnapIfNear(actionY, 0.74f, snapThreshold);
				break;
			}
		}

		private static float SnapIfNear(float value, float anchor, float threshold)
		{
			if (Math.Abs(value - anchor) <= threshold)
			{
				return anchor;
			}

			return value;
		}

		private void QueueLayoutUpdate(bool includeScale)
		{
			pendingLayoutUpdate = true;
			pendingScaleUpdate = pendingScaleUpdate || includeScale;
		}

		private void FlushLayoutUpdate(bool force)
		{
			if (!pendingLayoutUpdate)
			{
				return;
			}

			pendingApplyTimer += Engine.RawDeltaTime;
			if (!force && pendingApplyTimer < 0.05f)
			{
				return;
			}

			PushLayoutUpdate(pendingScaleUpdate);
			pendingLayoutUpdate = false;
			pendingScaleUpdate = false;
			pendingApplyTimer = 0f;
		}

		private void PushLayoutUpdate(bool includeScale)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.TouchLeftStickX = leftX;
				update.TouchLeftStickY = leftY;
				update.TouchActionX = actionX;
				update.TouchActionY = actionY;
				update.TouchShoulderY = shoulderY;
				update.TouchStartSelectY = startSelectY;
				update.TouchLeftStickRadius = leftStickRadius;
				update.TouchButtonRadius = buttonRadius;
				update.TouchActionSpacing = actionSpacing;
				if (includeScale)
				{
					update.TouchScale = scale;
				}
			});
		}

		private void ResetLayoutToDefaults()
		{
			leftX = 0.18f;
			leftY = 0.76f;
			actionX = 0.82f;
			actionY = 0.74f;
			shoulderY = 0.13f;
			startSelectY = 0.12f;
			scale = 1f;
			leftStickRadius = 0.12f;
			buttonRadius = 0.07f;
			actionSpacing = 1.35f;
			QueueLayoutUpdate(includeScale: true);
			FlushLayoutUpdate(force: true);
		}

		private void ResolvePreviewLayout(float width, float height, out Vector2 leftCenter, out Vector2 shoulderCenter, out Vector2 startSelectCenter, out Vector2 actionCenter, out Vector2 aCenter, out Vector2 bCenter, out Vector2 xCenter, out Vector2 yCenter, out float leftRadiusPx, out float buttonRadiusPx)
		{
			float minDimension = MathF.Min(width, height);
			leftRadiusPx = minDimension * Math.Clamp(leftStickRadius, 0.08f, 0.2f) * Math.Clamp(scale, 0.65f, 1.8f);
			buttonRadiusPx = minDimension * Math.Clamp(buttonRadius, 0.05f, 0.14f) * Math.Clamp(scale, 0.65f, 1.8f);
			float spacing = buttonRadiusPx * Math.Clamp(actionSpacing, 1.05f, 2f);

			leftCenter = new Vector2(width * Math.Clamp(leftX, 0.06f, 0.45f), height * Math.Clamp(leftY, 0.4f, 0.95f));
			shoulderCenter = new Vector2(width * 0.5f, height * Math.Clamp(shoulderY, 0.06f, 0.3f));
			startSelectCenter = new Vector2(width * 0.5f, height * Math.Clamp(startSelectY, 0.06f, 0.3f));
			actionCenter = new Vector2(width * Math.Clamp(actionX, 0.52f, 0.95f), height * Math.Clamp(actionY, 0.4f, 0.95f));
			aCenter = actionCenter + new Vector2(0f, spacing);
			bCenter = actionCenter + new Vector2(spacing, 0f);
			xCenter = actionCenter + new Vector2(-spacing, 0f);
			yCenter = actionCenter + new Vector2(0f, -spacing);
		}

		private static bool SetClamped(ref float target, float next, float min, float max)
		{
			float clamped = Math.Clamp(next, min, max);
			if (Math.Abs(target - clamped) <= 0.0005f)
			{
				return false;
			}

			target = clamped;
			return true;
		}

		private static bool TryGetPinchDistance(TouchCollection touches, out float distance)
		{
			distance = 0f;
			Vector2 first = default(Vector2);
			Vector2 second = default(Vector2);
			bool firstSet = false;
			bool secondSet = false;

			for (int i = 0; i < touches.Count; i++)
			{
				TouchLocation touch = touches[i];
				if (!IsTouchDown(touch.State))
				{
					continue;
				}

				if (!firstSet)
				{
					first = touch.Position;
					firstSet = true;
					continue;
				}

				second = touch.Position;
				secondSet = true;
				break;
			}

			if (!firstSet || !secondSet)
			{
				return false;
			}

			distance = Vector2.Distance(first, second);
			return true;
		}

		private static bool IsTouchDown(TouchLocationState state)
		{
			return state != TouchLocationState.Released && state != TouchLocationState.Invalid;
		}
	}

	public static TextMenu Create(bool inGame = false, EventInstance snapshot = null)
	{
		MenuOptions.inGame = inGame;
		MenuOptions.snapshot = snapshot;
		menu = new TextMenu();
		menu.Add(new TextMenu.Header(Dialog.Clean("options_title")));
		menu.OnClose = delegate
		{
			crouchDashMode = null;
		};
		if (!inGame && Dialog.Languages.Count > 1)
		{
			menu.Add(new TextMenu.SubHeader(""));
			TextMenu.LanguageButton languageButton = new TextMenu.LanguageButton(Dialog.Clean("options_language"), Dialog.Language);
			languageButton.Pressed(SelectLanguage);
			menu.Add(languageButton);
		}
		menu.Add(new TextMenu.SubHeader(Dialog.Clean("options_controls")));
		CreateRumble(menu);
		CreateGrabMode(menu);
		crouchDashMode = CreateCrouchDashMode(menu);
		menu.Add(new TextMenu.Button(Dialog.Clean("options_keyconfig")).Pressed(OpenKeyboardConfig));
		menu.Add(new TextMenu.Button(Dialog.Clean("options_btnconfig")).Pressed(OpenButtonConfig));
		menu.Add(new TextMenu.SubHeader(Dialog.Clean("options_video")));
		menu.Add(new TextMenu.OnOff(Dialog.Clean("options_fullscreen"), Settings.Instance.Fullscreen).Change(SetFullscreen));
		menu.Add(window = new TextMenu.Slider(Dialog.Clean("options_window"), (int i) => i + "x", 3, 10, Settings.Instance.WindowScale).Change(SetWindow));
		menu.Add(new TextMenu.OnOff(Dialog.Clean("options_vsync"), Settings.Instance.VSync).Change(SetVSync));
		menu.Add(new TextMenu.OnOff(Dialog.Clean("OPTIONS_DISABLE_FLASH"), Settings.Instance.DisableFlashes).Change(delegate(bool b)
		{
			Settings.Instance.DisableFlashes = b;
		}));
		menu.Add(new TextMenu.Slider(Dialog.Clean("OPTIONS_DISABLE_SHAKE"), (int i) => i switch
		{
			2 => Dialog.Clean("OPTIONS_RUMBLE_ON"), 
			1 => Dialog.Clean("OPTIONS_RUMBLE_HALF"), 
			_ => Dialog.Clean("OPTIONS_RUMBLE_OFF"), 
		}, 0, 2, (int)Settings.Instance.ScreenShake).Change(delegate(int i)
		{
			Settings.Instance.ScreenShake = (ScreenshakeAmount)i;
		}));
		menu.Add(viewport = new TextMenu.Button(Dialog.Clean("OPTIONS_VIEWPORT_PC")).Pressed(OpenViewportAdjustment));
		menu.Add(new TextMenu.SubHeader(Dialog.Clean("options_audio")));
		menu.Add(new TextMenu.Slider(Dialog.Clean("options_music"), (int i) => i.ToString(), 0, 10, Settings.Instance.MusicVolume).Change(SetMusic).Enter(EnterSound).Leave(LeaveSound));
		menu.Add(new TextMenu.Slider(Dialog.Clean("options_sounds"), (int i) => i.ToString(), 0, 10, Settings.Instance.SFXVolume).Change(SetSfx).Enter(EnterSound).Leave(LeaveSound));
		menu.Add(new TextMenu.SubHeader(Dialog.Clean("options_gameplay")));
		menu.Add(new TextMenu.Slider(Dialog.Clean("options_speedrun"), (int i) => i switch
		{
			0 => Dialog.Get("OPTIONS_OFF"), 
			1 => Dialog.Get("OPTIONS_SPEEDRUN_CHAPTER"), 
			_ => Dialog.Get("OPTIONS_SPEEDRUN_FILE"), 
		}, 0, 2, (int)Settings.Instance.SpeedrunClock).Change(SetSpeedrunClock));
		CreateRuntimeOptions(menu);
		viewport.Visible = Settings.Instance.Fullscreen;
		if (window != null)
		{
			window.Visible = !Settings.Instance.Fullscreen;
		}
		if (menu.Height > menu.ScrollableMinSize)
		{
			menu.Position.Y = menu.ScrollTargetY;
		}
		return menu;
	}

	private static void CreateRumble(TextMenu menu)
	{
		menu.Add(new TextMenu.Slider(Dialog.Clean("options_rumble_PC"), (int i) => i switch
		{
			2 => Dialog.Clean("OPTIONS_RUMBLE_ON"), 
			1 => Dialog.Clean("OPTIONS_RUMBLE_HALF"), 
			_ => Dialog.Clean("OPTIONS_RUMBLE_OFF"), 
		}, 0, 2, (int)Settings.Instance.Rumble).Change(delegate(int i)
		{
			Settings.Instance.Rumble = (RumbleAmount)i;
			Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
		}));
	}

	private static void CreateGrabMode(TextMenu menu)
	{
		menu.Add(new TextMenu.Slider(Dialog.Clean("OPTIONS_GRAB_MODE"), (int i) => i switch
		{
			0 => Dialog.Clean("OPTIONS_BUTTON_HOLD"), 
			1 => Dialog.Clean("OPTIONS_BUTTON_INVERT"), 
			_ => Dialog.Clean("OPTIONS_BUTTON_TOGGLE"), 
		}, 0, 2, (int)Settings.Instance.GrabMode).Change(delegate(int i)
		{
			Settings.Instance.GrabMode = (GrabModes)i;
			Input.ResetGrab();
		}));
	}

	private static TextMenu.Item CreateCrouchDashMode(TextMenu menu)
	{
		TextMenu.Option<int> option = new TextMenu.Slider(Dialog.Clean("OPTIONS_CROUCH_DASH_MODE"), (int i) => (i == 0) ? Dialog.Clean("OPTIONS_BUTTON_PRESS") : Dialog.Clean("OPTIONS_BUTTON_HOLD"), 0, 1, (int)Settings.Instance.CrouchDashMode).Change(delegate(int i)
		{
			Settings.Instance.CrouchDashMode = (CrouchDashModes)i;
		});
		option.Visible = Input.CrouchDash.Binding.HasInput;
		menu.Add(option);
		return option;
	}

	private static void CreateRuntimeOptions(TextMenu menu)
	{
		if (!CelesteRuntimeConfigBridge.TryGetSnapshot(out RuntimeUiConfigSnapshot snapshot))
		{
			return;
		}

		menu.Add(new TextMenu.SubHeader(RuntimeText("runtime_section")));
		menu.Add(new TextMenu.SubHeader(RuntimeText("runtime_game"), topPadding: false));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_vsync"), snapshot.VSync).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.VSync = on;
			});
		}));

		int targetFpsIndex = FindStringIndex(RuntimeTargetFpsValues, snapshot.TargetFps, fallbackIndex: 1);
		menu.Add(new TextMenu.Slider(RuntimeText("runtime_target_fps"), (int i) => (i >= 0 && i < RuntimeTargetFpsValues.Length) ? ((RuntimeTargetFpsValues[i] == "Uncapped") ? RuntimeText("runtime_value_uncapped") : RuntimeTargetFpsValues[i]) : "60", 0, RuntimeTargetFpsValues.Length - 1, targetFpsIndex).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.TargetFps = RuntimeTargetFpsValues[Math.Clamp(i, 0, RuntimeTargetFpsValues.Length - 1)];
			});
		}));

		menu.Add(new TextMenu.Slider(RuntimeText("runtime_aspect"), (int i) => i switch
		{
			0 => RuntimeText("runtime_value_fit"), 
			1 => RuntimeText("runtime_value_fill"), 
			_ => RuntimeText("runtime_value_stretch"), 
		}, 0, 2, (int)snapshot.AspectMode).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.AspectMode = (RuntimeUiAspectModes)Math.Clamp(i, 0, 2);
			});
		}));

		menu.Add(new TextMenu.Slider(RuntimeText("runtime_scale_filter"), (int i) => (i == 0) ? RuntimeText("runtime_value_point") : RuntimeText("runtime_value_linear"), 0, 1, (int)snapshot.ScaleFilter).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ScaleFilter = (RuntimeUiScaleFilters)Math.Clamp(i, 0, 1);
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_bloom"), snapshot.Bloom).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.Bloom = on;
			});
		}));

		menu.Add(new TextMenu.Slider(RuntimeText("runtime_post"), (int i) => i switch
		{
			0 => RuntimeText("runtime_value_low"), 
			1 => RuntimeText("runtime_value_medium"), 
			_ => RuntimeText("runtime_value_high"), 
		}, 0, 2, (int)snapshot.PostProcessingQuality).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.PostProcessingQuality = (RuntimeUiQualityLevels)Math.Clamp(i, 0, 2);
			});
		}));

		menu.Add(new TextMenu.Slider(RuntimeText("runtime_particles"), (int i) => i switch
		{
			0 => RuntimeText("runtime_value_low"), 
			1 => RuntimeText("runtime_value_medium"), 
			_ => RuntimeText("runtime_value_high"), 
		}, 0, 2, (int)snapshot.Particles).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.Particles = (RuntimeUiQualityLevels)Math.Clamp(i, 0, 2);
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_compat"), snapshot.ForceCompatibilityCompositor).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ForceCompatibilityCompositor = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_legacy"), snapshot.ForceLegacyBlendStates).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ForceLegacyBlendStates = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_edge"), snapshot.UseEdgeToEdgeOnAndroid).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.UseEdgeToEdgeOnAndroid = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_logs"), snapshot.EnableDiagnosticLogs).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.EnableDiagnosticLogs = on;
			});
		}));

		menu.Add(new TextMenu.SubHeader(RuntimeText("runtime_overlay")));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_show_fps"), snapshot.ShowFps).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ShowFps = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_show_memory"), snapshot.ShowMemory).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ShowMemory = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_show_resolution"), snapshot.ShowResolution).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ShowResolution = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_show_viewport"), snapshot.ShowViewport).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ShowViewport = on;
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_show_scale"), snapshot.ShowScale).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.ShowScale = on;
			});
		}));

		menu.Add(new TextMenu.Slider(RuntimeText("runtime_overlay_position"), (int i) => i switch
		{
			0 => RuntimeText("runtime_value_topleft"), 
			1 => RuntimeText("runtime_value_topright"), 
			2 => RuntimeText("runtime_value_bottomleft"), 
			_ => RuntimeText("runtime_value_bottomright"), 
		}, 0, 3, (int)snapshot.OverlayPosition).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.OverlayPosition = (RuntimeUiOverlayPositions)Math.Clamp(i, 0, 3);
			});
		}));

		int fontScaleIndex = FindNearestIndex(RuntimeOverlayFontScaleValues, snapshot.OverlayFontScale);
		menu.Add(new TextMenu.Slider(RuntimeText("runtime_overlay_font_scale"), (int i) => (RuntimeOverlayFontScaleValues[Math.Clamp(i, 0, RuntimeOverlayFontScaleValues.Length - 1)] * 100f).ToString("0", CultureInfo.InvariantCulture) + "%", 0, RuntimeOverlayFontScaleValues.Length - 1, fontScaleIndex).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.OverlayFontScale = RuntimeOverlayFontScaleValues[Math.Clamp(i, 0, RuntimeOverlayFontScaleValues.Length - 1)];
			});
		}));

		int updateIntervalIndex = FindNearestIndex(RuntimeOverlayUpdateMsValues, snapshot.OverlayUpdateIntervalMs);
		menu.Add(new TextMenu.Slider(RuntimeText("runtime_overlay_update"), (int i) => RuntimeOverlayUpdateMsValues[Math.Clamp(i, 0, RuntimeOverlayUpdateMsValues.Length - 1)] + " ms", 0, RuntimeOverlayUpdateMsValues.Length - 1, updateIntervalIndex).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.OverlayUpdateIntervalMs = RuntimeOverlayUpdateMsValues[Math.Clamp(i, 0, RuntimeOverlayUpdateMsValues.Length - 1)];
			});
		}));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_overlay_background"), snapshot.OverlayBackground).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.OverlayBackground = on;
			});
		}));

		int paddingIndex = FindNearestIndex(RuntimeOverlayPaddingValues, snapshot.OverlayPadding);
		menu.Add(new TextMenu.Slider(RuntimeText("runtime_overlay_padding"), (int i) => RuntimeOverlayPaddingValues[Math.Clamp(i, 0, RuntimeOverlayPaddingValues.Length - 1)] + " px", 0, RuntimeOverlayPaddingValues.Length - 1, paddingIndex).Change(delegate(int i)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.OverlayPadding = RuntimeOverlayPaddingValues[Math.Clamp(i, 0, RuntimeOverlayPaddingValues.Length - 1)];
			});
		}));

		menu.Add(new TextMenu.SubHeader(RuntimeText("runtime_touch")));

		menu.Add(new TextMenu.OnOff(RuntimeText("runtime_touch_enabled"), snapshot.TouchEnabled).Change(delegate(bool on)
		{
			ApplyRuntimeUpdate(delegate(RuntimeUiConfigUpdate update)
			{
				update.TouchEnabled = on;
			});
		}));
		menu.Add(new TextMenu.Button(RuntimeText("runtime_touch_open")).Pressed(OpenTouchConfig));
	}

	private static void ApplyRuntimeUpdate(Action<RuntimeUiConfigUpdate> configure)
	{
		RuntimeUiConfigUpdate runtimeUiConfigUpdate = new RuntimeUiConfigUpdate();
		configure(runtimeUiConfigUpdate);
		CelesteRuntimeConfigBridge.TryApplyUpdate(runtimeUiConfigUpdate);
	}

	private static int FindStringIndex(string[] values, string value, int fallbackIndex)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return fallbackIndex;
		}

		for (int i = 0; i < values.Length; i++)
		{
			if (string.Equals(values[i], value, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}

		return fallbackIndex;
	}

	private static int FindNearestIndex(float[] values, float value)
	{
		int num = 0;
		float num2 = float.MaxValue;
		for (int i = 0; i < values.Length; i++)
		{
			float num3 = Math.Abs(values[i] - value);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}

		return num;
	}

	private static int FindNearestIndex(int[] values, int value)
	{
		int num = 0;
		int num2 = int.MaxValue;
		for (int i = 0; i < values.Length; i++)
		{
			int num3 = Math.Abs(values[i] - value);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}

		return num;
	}

	private static string RuntimeText(string key)
	{
		string text = Settings.Instance?.Language ?? "english";
		switch (text)
		{
		case "brazilian":
			return key switch
			{
				"runtime_section" => "RUNTIME ANDROID", 
				"runtime_game" => "CONFIGURACAO DO JOGO", 
				"runtime_overlay" => "CONFIGURACAO DO OVERLAY", 
				"runtime_vsync" => "VSync do Runtime", 
				"runtime_target_fps" => "FPS Alvo do Runtime", 
				"runtime_aspect" => "Modo de Aspecto", 
				"runtime_scale_filter" => "Filtro de Escala", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Pos-processamento", 
				"runtime_particles" => "Particulas", 
				"runtime_compat" => "Compositor de Compatibilidade", 
				"runtime_legacy" => "Blend Legado", 
				"runtime_edge" => "Tela sem Bordas", 
				"runtime_logs" => "Logs de Diagnostico", 
				"runtime_show_fps" => "Overlay: FPS", 
				"runtime_show_memory" => "Overlay: Memoria", 
				"runtime_show_resolution" => "Overlay: Resolucao", 
				"runtime_show_viewport" => "Overlay: Viewport", 
				"runtime_show_scale" => "Overlay: Escala", 
				"runtime_overlay_position" => "Posicao do Overlay", 
				"runtime_overlay_font_scale" => "Escala da Fonte", 
				"runtime_overlay_update" => "Atualizacao do Overlay", 
				"runtime_overlay_background" => "Fundo do Overlay", 
				"runtime_overlay_padding" => "Padding do Overlay", 
				"runtime_touch" => "CONTROLES TOUCH", 
				"runtime_touch_enabled" => "Touch Habilitado", 
				"runtime_touch_gameplay_only" => "Touch so no Mapa", 
				"runtime_touch_auto_disable" => "Auto desativar com HW", 
				"runtime_touch_menu_tap" => "Toque para Menus/Mapas", 
				"runtime_touch_dpad" => "Mostrar D-Pad", 
				"runtime_touch_shoulders" => "Mostrar LB/RB/LT/RT", 
				"runtime_touch_start_select" => "Mostrar Start/Back", 
				"runtime_touch_shoulder_y" => "Altura dos Ombros", 
				"runtime_touch_start_select_y" => "Altura Start/Back", 
				"runtime_touch_opacity" => "Opacidade do Touch", 
				"runtime_touch_scale" => "Escala do Touch", 
				"runtime_touch_deadzone" => "Deadzone do Analogo", 
				"runtime_touch_stick_radius" => "Tamanho do Analogo", 
				"runtime_touch_button_radius" => "Tamanho dos Botoes", 
				"runtime_touch_spacing" => "Espacamento ABXY", 
				"runtime_touch_left_x" => "Posicao X do Analogo", 
				"runtime_touch_left_y" => "Posicao Y do Analogo", 
				"runtime_touch_dpad_x" => "Posicao X do D-Pad", 
				"runtime_touch_dpad_y" => "Posicao Y do D-Pad", 
				"runtime_touch_action_x" => "Posicao X dos Botoes", 
				"runtime_touch_action_y" => "Posicao Y dos Botoes", 
				"runtime_touch_open" => "Abrir Ajustes Touch", 
				"runtime_touch_general" => "GERAL", 
				"runtime_touch_layout" => "VISUAL E HUD", 
				"runtime_touch_movement" => "MOVIMENTO", 
				"runtime_touch_actions" => "ACOES", 
				"runtime_touch_profile" => "Perfil de Botoes", 
				"runtime_touch_style" => "Estilo dos Icons", 
				"runtime_touch_editor_open" => "Editor Visual Touch", 
				"runtime_touch_editor_title" => "EDITOR VISUAL TOUCH", 
				"runtime_touch_editor_hint_drag" => "Arraste analogico esquerdo, botoes superiores e grupo ABX.", 
				"runtime_touch_editor_hint_pinch" => "Use pinch com dois dedos para ajustar escala.", 
				"runtime_touch_editor_reset" => "Redefinir", 
				"runtime_touch_editor_done" => "Concluir", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Precisao", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Ajustar", 
				"runtime_value_fill" => "Preencher", 
				"runtime_value_stretch" => "Esticar", 
				"runtime_value_point" => "Ponto", 
				"runtime_value_linear" => "Linear", 
				"runtime_value_low" => "Baixo", 
				"runtime_value_medium" => "Medio", 
				"runtime_value_high" => "Alto", 
				"runtime_value_uncapped" => "Sem Limite", 
				"runtime_value_topleft" => "Topo Esquerda", 
				"runtime_value_topright" => "Topo Direita", 
				"runtime_value_bottomleft" => "Baixo Esquerda", 
				"runtime_value_bottomright" => "Baixo Direita", 
				_ => key, 
			};
		case "spanish":
			return key switch
			{
				"runtime_section" => "RUNTIME ANDROID", 
				"runtime_game" => "CONFIGURACION DEL JUEGO", 
				"runtime_overlay" => "CONFIGURACION DEL OVERLAY", 
				"runtime_vsync" => "VSync de Runtime", 
				"runtime_target_fps" => "FPS Objetivo", 
				"runtime_aspect" => "Modo de Aspecto", 
				"runtime_scale_filter" => "Filtro de Escala", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Postprocesado", 
				"runtime_particles" => "Particulas", 
				"runtime_compat" => "Compositor de Compatibilidad", 
				"runtime_legacy" => "Blend Heredado", 
				"runtime_edge" => "Pantalla sin Bordes", 
				"runtime_logs" => "Logs de Diagnostico", 
				"runtime_show_fps" => "Overlay: FPS", 
				"runtime_show_memory" => "Overlay: Memoria", 
				"runtime_show_resolution" => "Overlay: Resolucion", 
				"runtime_show_viewport" => "Overlay: Viewport", 
				"runtime_show_scale" => "Overlay: Escala", 
				"runtime_overlay_position" => "Posicion del Overlay", 
				"runtime_overlay_font_scale" => "Escala de Fuente", 
				"runtime_overlay_update" => "Actualizacion del Overlay", 
				"runtime_overlay_background" => "Fondo del Overlay", 
				"runtime_overlay_padding" => "Padding del Overlay", 
				"runtime_touch" => "CONTROLES TOUCH", 
				"runtime_touch_enabled" => "Touch Activado", 
				"runtime_touch_gameplay_only" => "Touch solo en Juego", 
				"runtime_touch_auto_disable" => "Auto desactivar con HW", 
				"runtime_touch_menu_tap" => "Toque para Menus/Mapas", 
				"runtime_touch_dpad" => "Mostrar D-Pad", 
				"runtime_touch_shoulders" => "Mostrar LB/RB/LT/RT", 
				"runtime_touch_start_select" => "Mostrar Start/Back", 
				"runtime_touch_shoulder_y" => "Altura de Hombros", 
				"runtime_touch_start_select_y" => "Altura de Start/Back", 
				"runtime_touch_opacity" => "Opacidad Touch", 
				"runtime_touch_scale" => "Escala Touch", 
				"runtime_touch_deadzone" => "Zona Muerta Stick", 
				"runtime_touch_stick_radius" => "Tamano Stick", 
				"runtime_touch_button_radius" => "Tamano Botones", 
				"runtime_touch_spacing" => "Espaciado ABXY", 
				"runtime_touch_left_x" => "Stick Izquierdo X", 
				"runtime_touch_left_y" => "Stick Izquierdo Y", 
				"runtime_touch_dpad_x" => "D-Pad X", 
				"runtime_touch_dpad_y" => "D-Pad Y", 
				"runtime_touch_action_x" => "Botones X", 
				"runtime_touch_action_y" => "Botones Y", 
				"runtime_touch_open" => "Abrir Ajustes Touch", 
				"runtime_touch_general" => "GENERAL", 
				"runtime_touch_layout" => "VISUAL Y HUD", 
				"runtime_touch_movement" => "MOVIMIENTO", 
				"runtime_touch_actions" => "ACCIONES", 
				"runtime_touch_profile" => "Perfil de Botones", 
				"runtime_touch_style" => "Estilo de Iconos", 
				"runtime_touch_editor_open" => "Editor Visual Touch", 
				"runtime_touch_editor_title" => "EDITOR VISUAL TOUCH", 
				"runtime_touch_editor_hint_drag" => "Arrastra stick izquierdo, botones superiores y grupo ABX.", 
				"runtime_touch_editor_hint_pinch" => "Usa pinch con dos dedos para la escala.", 
				"runtime_touch_editor_reset" => "Restablecer", 
				"runtime_touch_editor_done" => "Listo", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Precision", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Ajustar", 
				"runtime_value_fill" => "Rellenar", 
				"runtime_value_stretch" => "Estirar", 
				"runtime_value_point" => "Punto", 
				"runtime_value_linear" => "Lineal", 
				"runtime_value_low" => "Bajo", 
				"runtime_value_medium" => "Medio", 
				"runtime_value_high" => "Alto", 
				"runtime_value_uncapped" => "Sin Limite", 
				"runtime_value_topleft" => "Arriba Izquierda", 
				"runtime_value_topright" => "Arriba Derecha", 
				"runtime_value_bottomleft" => "Abajo Izquierda", 
				"runtime_value_bottomright" => "Abajo Derecha", 
				_ => key, 
			};
		case "french":
			return key switch
			{
				"runtime_section" => "RUNTIME ANDROID", 
				"runtime_game" => "CONFIGURATION JEU", 
				"runtime_overlay" => "CONFIGURATION OVERLAY", 
				"runtime_vsync" => "VSync Runtime", 
				"runtime_target_fps" => "FPS Cible", 
				"runtime_aspect" => "Mode Aspect", 
				"runtime_scale_filter" => "Filtre d Echelle", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Post-traitement", 
				"runtime_particles" => "Particules", 
				"runtime_compat" => "Compositeur Compatibilite", 
				"runtime_legacy" => "Blend Legacy", 
				"runtime_edge" => "Ecran sans Bordures", 
				"runtime_logs" => "Logs Diagnostic", 
				"runtime_show_fps" => "Overlay : FPS", 
				"runtime_show_memory" => "Overlay : Memoire", 
				"runtime_show_resolution" => "Overlay : Resolution", 
				"runtime_show_viewport" => "Overlay : Viewport", 
				"runtime_show_scale" => "Overlay : Echelle", 
				"runtime_overlay_position" => "Position Overlay", 
				"runtime_overlay_font_scale" => "Echelle Police", 
				"runtime_overlay_update" => "Frequence Overlay", 
				"runtime_overlay_background" => "Fond Overlay", 
				"runtime_overlay_padding" => "Padding Overlay", 
				"runtime_touch" => "COMMANDES TACTILES", 
				"runtime_touch_enabled" => "Touch Active", 
				"runtime_touch_gameplay_only" => "Touch en Jeu Seulement", 
				"runtime_touch_auto_disable" => "Auto desactiver avec HW", 
				"runtime_touch_menu_tap" => "Toucher pour Menus/Cartes", 
				"runtime_touch_dpad" => "Afficher D-Pad", 
				"runtime_touch_shoulders" => "Afficher LB/RB/LT/RT", 
				"runtime_touch_start_select" => "Afficher Start/Back", 
				"runtime_touch_shoulder_y" => "Hauteur Epaules", 
				"runtime_touch_start_select_y" => "Hauteur Start/Back", 
				"runtime_touch_opacity" => "Opacite Touch", 
				"runtime_touch_scale" => "Echelle Touch", 
				"runtime_touch_deadzone" => "Zone Morte Stick", 
				"runtime_touch_stick_radius" => "Taille Stick", 
				"runtime_touch_button_radius" => "Taille Boutons", 
				"runtime_touch_spacing" => "Espacement ABXY", 
				"runtime_touch_left_x" => "Stick Gauche X", 
				"runtime_touch_left_y" => "Stick Gauche Y", 
				"runtime_touch_dpad_x" => "D-Pad X", 
				"runtime_touch_dpad_y" => "D-Pad Y", 
				"runtime_touch_action_x" => "Boutons X", 
				"runtime_touch_action_y" => "Boutons Y", 
				"runtime_touch_open" => "Ouvrir Reglages Touch", 
				"runtime_touch_general" => "GENERAL", 
				"runtime_touch_layout" => "VISUEL ET HUD", 
				"runtime_touch_movement" => "MOUVEMENT", 
				"runtime_touch_actions" => "ACTIONS", 
				"runtime_touch_profile" => "Profil des Boutons", 
				"runtime_touch_style" => "Style des Icones", 
				"runtime_touch_editor_open" => "Editeur Visuel Touch", 
				"runtime_touch_editor_title" => "EDITEUR VISUEL TOUCH", 
				"runtime_touch_editor_hint_drag" => "Glissez stick gauche, boutons du haut et groupe ABX.", 
				"runtime_touch_editor_hint_pinch" => "Utilisez un pinch a deux doigts pour l echelle.", 
				"runtime_touch_editor_reset" => "Reinitialiser", 
				"runtime_touch_editor_done" => "Terminer", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Precision", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Ajuster", 
				"runtime_value_fill" => "Remplir", 
				"runtime_value_stretch" => "Etirer", 
				"runtime_value_point" => "Point", 
				"runtime_value_linear" => "Lineaire", 
				"runtime_value_low" => "Bas", 
				"runtime_value_medium" => "Moyen", 
				"runtime_value_high" => "Eleve", 
				"runtime_value_uncapped" => "Illimite", 
				"runtime_value_topleft" => "Haut Gauche", 
				"runtime_value_topright" => "Haut Droite", 
				"runtime_value_bottomleft" => "Bas Gauche", 
				"runtime_value_bottomright" => "Bas Droite", 
				_ => key, 
			};
		case "german":
			return key switch
			{
				"runtime_section" => "ANDROID RUNTIME", 
				"runtime_game" => "SPIEL KONFIG", 
				"runtime_overlay" => "OVERLAY KONFIG", 
				"runtime_vsync" => "Runtime VSync", 
				"runtime_target_fps" => "Ziel FPS", 
				"runtime_aspect" => "Seitenverhaltnis", 
				"runtime_scale_filter" => "Skalierungsfilter", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Post Processing", 
				"runtime_particles" => "Partikel", 
				"runtime_compat" => "Kompatibilitats Compositor", 
				"runtime_legacy" => "Legacy Blend", 
				"runtime_edge" => "Randloser Bildschirm", 
				"runtime_logs" => "Diagnose Logs", 
				"runtime_show_fps" => "Overlay: FPS", 
				"runtime_show_memory" => "Overlay: Speicher", 
				"runtime_show_resolution" => "Overlay: Auflosung", 
				"runtime_show_viewport" => "Overlay: Viewport", 
				"runtime_show_scale" => "Overlay: Skalierung", 
				"runtime_overlay_position" => "Overlay Position", 
				"runtime_overlay_font_scale" => "Overlay Schriftgrosse", 
				"runtime_overlay_update" => "Overlay Aktualisierung", 
				"runtime_overlay_background" => "Overlay Hintergrund", 
				"runtime_overlay_padding" => "Overlay Padding", 
				"runtime_touch" => "TOUCH-STEUERUNG", 
				"runtime_touch_enabled" => "Touch Aktiviert", 
				"runtime_touch_gameplay_only" => "Touch nur im Spiel", 
				"runtime_touch_auto_disable" => "Auto Deaktivieren bei HW", 
				"runtime_touch_menu_tap" => "Tippen fur Menus/Karten", 
				"runtime_touch_dpad" => "D-Pad Anzeigen", 
				"runtime_touch_shoulders" => "LB/RB/LT/RT Anzeigen", 
				"runtime_touch_start_select" => "Start/Back Anzeigen", 
				"runtime_touch_shoulder_y" => "Schulter-Hohe", 
				"runtime_touch_start_select_y" => "Start/Back-Hohe", 
				"runtime_touch_opacity" => "Touch Opazitat", 
				"runtime_touch_scale" => "Touch Skalierung", 
				"runtime_touch_deadzone" => "Stick Deadzone", 
				"runtime_touch_stick_radius" => "Stick Grosse", 
				"runtime_touch_button_radius" => "Button Grosse", 
				"runtime_touch_spacing" => "ABXY Abstand", 
				"runtime_touch_left_x" => "Linker Stick X", 
				"runtime_touch_left_y" => "Linker Stick Y", 
				"runtime_touch_dpad_x" => "D-Pad X", 
				"runtime_touch_dpad_y" => "D-Pad Y", 
				"runtime_touch_action_x" => "Buttons X", 
				"runtime_touch_action_y" => "Buttons Y", 
				"runtime_touch_open" => "Touch-Einstellungen Offnen", 
				"runtime_touch_general" => "ALLGEMEIN", 
				"runtime_touch_layout" => "ANZEIGE UND HUD", 
				"runtime_touch_movement" => "BEWEGUNG", 
				"runtime_touch_actions" => "AKTIONEN", 
				"runtime_touch_profile" => "Button-Profil", 
				"runtime_touch_style" => "Icon-Stil", 
				"runtime_touch_editor_open" => "Touch-Layout Editor", 
				"runtime_touch_editor_title" => "TOUCH LAYOUT EDITOR", 
				"runtime_touch_editor_hint_drag" => "Ziehe linken Stick, obere Tasten und ABX-Gruppe.", 
				"runtime_touch_editor_hint_pinch" => "Nutze Zwei-Finger-Pinch fur die Skalierung.", 
				"runtime_touch_editor_reset" => "Zurucksetzen", 
				"runtime_touch_editor_done" => "Fertig", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Prazision", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Anpassen", 
				"runtime_value_fill" => "Fullen", 
				"runtime_value_stretch" => "Strecken", 
				"runtime_value_point" => "Punkt", 
				"runtime_value_linear" => "Linear", 
				"runtime_value_low" => "Niedrig", 
				"runtime_value_medium" => "Mittel", 
				"runtime_value_high" => "Hoch", 
				"runtime_value_uncapped" => "Unbegrenzt", 
				"runtime_value_topleft" => "Oben Links", 
				"runtime_value_topright" => "Oben Rechts", 
				"runtime_value_bottomleft" => "Unten Links", 
				"runtime_value_bottomright" => "Unten Rechts", 
				_ => key, 
			};
		case "italian":
			return key switch
			{
				"runtime_section" => "RUNTIME ANDROID", 
				"runtime_game" => "CONFIGURAZIONE GIOCO", 
				"runtime_overlay" => "CONFIGURAZIONE OVERLAY", 
				"runtime_vsync" => "VSync Runtime", 
				"runtime_target_fps" => "FPS Obiettivo", 
				"runtime_aspect" => "Modalita Aspetto", 
				"runtime_scale_filter" => "Filtro Scala", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Post Processing", 
				"runtime_particles" => "Particelle", 
				"runtime_compat" => "Compositore Compatibilita", 
				"runtime_legacy" => "Blend Legacy", 
				"runtime_edge" => "Schermo senza Bordi", 
				"runtime_logs" => "Log Diagnostici", 
				"runtime_show_fps" => "Overlay: FPS", 
				"runtime_show_memory" => "Overlay: Memoria", 
				"runtime_show_resolution" => "Overlay: Risoluzione", 
				"runtime_show_viewport" => "Overlay: Viewport", 
				"runtime_show_scale" => "Overlay: Scala", 
				"runtime_overlay_position" => "Posizione Overlay", 
				"runtime_overlay_font_scale" => "Scala Font Overlay", 
				"runtime_overlay_update" => "Aggiornamento Overlay", 
				"runtime_overlay_background" => "Sfondo Overlay", 
				"runtime_overlay_padding" => "Padding Overlay", 
				"runtime_touch" => "CONTROLLI TOUCH", 
				"runtime_touch_enabled" => "Touch Abilitato", 
				"runtime_touch_gameplay_only" => "Touch solo in Gioco", 
				"runtime_touch_auto_disable" => "Auto disattiva con HW", 
				"runtime_touch_menu_tap" => "Tocco per Menu/Mappe", 
				"runtime_touch_dpad" => "Mostra D-Pad", 
				"runtime_touch_shoulders" => "Mostra LB/RB/LT/RT", 
				"runtime_touch_start_select" => "Mostra Start/Back", 
				"runtime_touch_shoulder_y" => "Altezza Spalle", 
				"runtime_touch_start_select_y" => "Altezza Start/Back", 
				"runtime_touch_opacity" => "Opacita Touch", 
				"runtime_touch_scale" => "Scala Touch", 
				"runtime_touch_deadzone" => "Deadzone Stick", 
				"runtime_touch_stick_radius" => "Dimensione Stick", 
				"runtime_touch_button_radius" => "Dimensione Pulsanti", 
				"runtime_touch_spacing" => "Spaziatura ABXY", 
				"runtime_touch_left_x" => "Stick Sinistro X", 
				"runtime_touch_left_y" => "Stick Sinistro Y", 
				"runtime_touch_dpad_x" => "D-Pad X", 
				"runtime_touch_dpad_y" => "D-Pad Y", 
				"runtime_touch_action_x" => "Pulsanti X", 
				"runtime_touch_action_y" => "Pulsanti Y", 
				"runtime_touch_open" => "Apri Impostazioni Touch", 
				"runtime_touch_general" => "GENERALE", 
				"runtime_touch_layout" => "VISUALE E HUD", 
				"runtime_touch_movement" => "MOVIMENTO", 
				"runtime_touch_actions" => "AZIONI", 
				"runtime_touch_profile" => "Profilo Pulsanti", 
				"runtime_touch_style" => "Stile Icone", 
				"runtime_touch_editor_open" => "Editor Visuale Touch", 
				"runtime_touch_editor_title" => "EDITOR VISUALE TOUCH", 
				"runtime_touch_editor_hint_drag" => "Trascina stick sinistro, tasti superiori e gruppo ABX.", 
				"runtime_touch_editor_hint_pinch" => "Usa pinch a due dita per la scala.", 
				"runtime_touch_editor_reset" => "Reimposta", 
				"runtime_touch_editor_done" => "Fine", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Precisione", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Adatta", 
				"runtime_value_fill" => "Riempi", 
				"runtime_value_stretch" => "Allunga", 
				"runtime_value_point" => "Punto", 
				"runtime_value_linear" => "Lineare", 
				"runtime_value_low" => "Basso", 
				"runtime_value_medium" => "Medio", 
				"runtime_value_high" => "Alto", 
				"runtime_value_uncapped" => "Senza Limite", 
				"runtime_value_topleft" => "Alto Sinistra", 
				"runtime_value_topright" => "Alto Destra", 
				"runtime_value_bottomleft" => "Basso Sinistra", 
				"runtime_value_bottomright" => "Basso Destra", 
				_ => key, 
			};
		default:
			return key switch
			{
				"runtime_section" => "ANDROID RUNTIME", 
				"runtime_game" => "GAME CONFIG", 
				"runtime_overlay" => "OVERLAY CONFIG", 
				"runtime_vsync" => "Runtime VSync", 
				"runtime_target_fps" => "Runtime Target FPS", 
				"runtime_aspect" => "Aspect Mode", 
				"runtime_scale_filter" => "Scale Filter", 
				"runtime_bloom" => "Bloom", 
				"runtime_post" => "Post Processing", 
				"runtime_particles" => "Particles", 
				"runtime_compat" => "Compatibility Compositor", 
				"runtime_legacy" => "Legacy Blend States", 
				"runtime_edge" => "Edge-to-Edge", 
				"runtime_logs" => "Diagnostic Logs", 
				"runtime_show_fps" => "Overlay: FPS", 
				"runtime_show_memory" => "Overlay: Memory", 
				"runtime_show_resolution" => "Overlay: Resolution", 
				"runtime_show_viewport" => "Overlay: Viewport", 
				"runtime_show_scale" => "Overlay: Scale", 
				"runtime_overlay_position" => "Overlay Position", 
				"runtime_overlay_font_scale" => "Overlay Font Scale", 
				"runtime_overlay_update" => "Overlay Refresh", 
				"runtime_overlay_background" => "Overlay Background", 
				"runtime_overlay_padding" => "Overlay Padding", 
				"runtime_touch" => "TOUCH CONTROLS", 
				"runtime_touch_enabled" => "Touch Enabled", 
				"runtime_touch_gameplay_only" => "Touch in Gameplay Only", 
				"runtime_touch_auto_disable" => "Auto Disable on Hardware", 
				"runtime_touch_menu_tap" => "Tap Navigation in Menus", 
				"runtime_touch_dpad" => "Show D-Pad", 
				"runtime_touch_shoulders" => "Show LB/RB/LT/RT", 
				"runtime_touch_start_select" => "Show Start/Back", 
				"runtime_touch_shoulder_y" => "Shoulders Y", 
				"runtime_touch_start_select_y" => "Start/Back Y", 
				"runtime_touch_opacity" => "Touch Opacity", 
				"runtime_touch_scale" => "Touch Scale", 
				"runtime_touch_deadzone" => "Stick Deadzone", 
				"runtime_touch_stick_radius" => "Stick Radius", 
				"runtime_touch_button_radius" => "Button Radius", 
				"runtime_touch_spacing" => "ABXY Spacing", 
				"runtime_touch_left_x" => "Left Stick X", 
				"runtime_touch_left_y" => "Left Stick Y", 
				"runtime_touch_dpad_x" => "D-Pad X", 
				"runtime_touch_dpad_y" => "D-Pad Y", 
				"runtime_touch_action_x" => "Face Buttons X", 
				"runtime_touch_action_y" => "Face Buttons Y", 
				"runtime_touch_open" => "Open Touch Settings", 
				"runtime_touch_general" => "GENERAL", 
				"runtime_touch_layout" => "VISUAL AND HUD", 
				"runtime_touch_movement" => "MOVEMENT", 
				"runtime_touch_actions" => "ACTIONS", 
				"runtime_touch_profile" => "Button Profile", 
				"runtime_touch_style" => "Icon Style", 
				"runtime_touch_editor_open" => "Open Touch Layout Editor", 
				"runtime_touch_editor_title" => "TOUCH LAYOUT EDITOR", 
				"runtime_touch_editor_hint_drag" => "Drag left stick, top buttons, and ABX cluster.", 
				"runtime_touch_editor_hint_pinch" => "Use two-finger pinch to change scale.", 
				"runtime_touch_editor_reset" => "Reset", 
				"runtime_touch_editor_done" => "Done", 
				"runtime_touch_editor_snap" => "Snap", 
				"runtime_touch_editor_precision" => "Precision", 
				"runtime_value_alt" => "Alt", 
				"runtime_value_alt_2" => "Alt 2", 
				"runtime_value_on" => "ON", 
				"runtime_value_off" => "OFF", 
				"runtime_value_xbox" => "Xbox", 
				"runtime_value_playstation" => "PlayStation", 
				"runtime_value_fit" => "Fit", 
				"runtime_value_fill" => "Fill", 
				"runtime_value_stretch" => "Stretch", 
				"runtime_value_point" => "Point", 
				"runtime_value_linear" => "Linear", 
				"runtime_value_low" => "Low", 
				"runtime_value_medium" => "Medium", 
				"runtime_value_high" => "High", 
				"runtime_value_uncapped" => "Uncapped", 
				"runtime_value_topleft" => "Top Left", 
				"runtime_value_topright" => "Top Right", 
				"runtime_value_bottomleft" => "Bottom Left", 
				"runtime_value_bottomright" => "Bottom Right", 
				_ => key, 
			};
		}
	}

	private static void SetFullscreen(bool on)
	{
		Settings.Instance.Fullscreen = on;
		Settings.Instance.ApplyScreen();
		if (window != null)
		{
			window.Visible = !on;
		}
		if (viewport != null)
		{
			viewport.Visible = on;
		}
	}

	private static void SetVSync(bool on)
	{
		Settings.Instance.VSync = on;
		Engine.Graphics.SynchronizeWithVerticalRetrace = Settings.Instance.VSync;
		Engine.Graphics.ApplyChanges();
	}

	private static void SetWindow(int scale)
	{
		Settings.Instance.WindowScale = scale;
		Settings.Instance.ApplyScreen();
	}

	private static void SetMusic(int volume)
	{
		Settings.Instance.MusicVolume = volume;
		Settings.Instance.ApplyMusicVolume();
	}

	private static void SetSfx(int volume)
	{
		Settings.Instance.SFXVolume = volume;
		Settings.Instance.ApplySFXVolume();
	}

	private static void SetSpeedrunClock(int val)
	{
		Settings.Instance.SpeedrunClock = (SpeedrunType)val;
	}

	private static void OpenViewportAdjustment()
	{
		if (Engine.Scene is Overworld)
		{
			(Engine.Scene as Overworld).ShowInputUI = false;
		}
		menu.Visible = false;
		menu.Focused = false;
		ViewportAdjustmentUI viewportAdjustmentUI = new ViewportAdjustmentUI();
		viewportAdjustmentUI.OnClose = delegate
		{
			menu.Visible = true;
			menu.Focused = true;
			if (Engine.Scene is Overworld)
			{
				(Engine.Scene as Overworld).ShowInputUI = true;
			}
		};
		Engine.Scene.Add(viewportAdjustmentUI);
		Engine.Scene.OnEndOfFrame += delegate
		{
			Engine.Scene.Entities.UpdateLists();
		};
	}

	private static void SelectLanguage()
	{
		menu.Focused = false;
		LanguageSelectUI languageSelectUI = new LanguageSelectUI();
		languageSelectUI.OnClose = delegate
		{
			menu.Focused = true;
		};
		Engine.Scene.Add(languageSelectUI);
		Engine.Scene.OnEndOfFrame += delegate
		{
			Engine.Scene.Entities.UpdateLists();
		};
	}

	private static void OpenKeyboardConfig()
	{
		menu.Focused = false;
		KeyboardConfigUI keyboardConfigUI = new KeyboardConfigUI();
		keyboardConfigUI.OnClose = delegate
		{
			menu.Focused = true;
		};
		Engine.Scene.Add(keyboardConfigUI);
		Engine.Scene.OnEndOfFrame += delegate
		{
			Engine.Scene.Entities.UpdateLists();
		};
	}

	private static void OpenButtonConfig()
	{
		menu.Focused = false;
		if (Engine.Scene is Overworld)
		{
			(Engine.Scene as Overworld).ShowConfirmUI = false;
		}
		ButtonConfigUI buttonConfigUI = new ButtonConfigUI();
		buttonConfigUI.OnClose = delegate
		{
			menu.Focused = true;
			if (Engine.Scene is Overworld)
			{
				(Engine.Scene as Overworld).ShowConfirmUI = true;
			}
		};
		Engine.Scene.Add(buttonConfigUI);
		Engine.Scene.OnEndOfFrame += delegate
		{
			Engine.Scene.Entities.UpdateLists();
		};
	}

	private static void OpenTouchConfig()
	{
		menu.Focused = false;
		TouchSettingsUI touchSettingsUI = new TouchSettingsUI();
		touchSettingsUI.OnClose = delegate
		{
			menu.Focused = true;
		};
		Engine.Scene.Add(touchSettingsUI);
		Engine.Scene.OnEndOfFrame += delegate
		{
			Engine.Scene.Entities.UpdateLists();
		};
	}

	private static void EnterSound()
	{
		if (inGame && snapshot != null)
		{
			Audio.EndSnapshot(snapshot);
		}
	}

	private static void LeaveSound()
	{
		if (inGame && snapshot != null)
		{
			Audio.ResumeSnapshot(snapshot);
		}
	}

	public static void UpdateCrouchDashModeVisibility()
	{
		if (crouchDashMode != null)
		{
			crouchDashMode.Visible = Input.CrouchDash.Binding.HasInput;
		}
	}
}
