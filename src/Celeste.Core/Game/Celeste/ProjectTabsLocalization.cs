using System;
using System.Collections.Generic;

namespace Celeste;

public static class ProjectTabsLocalization
{
    public const string ModsStatusDevKey = "mods_tab_status_dev";

    private static readonly Dictionary<string, Dictionary<string, string>> TextByLanguage = new(StringComparer.OrdinalIgnoreCase)
    {
        ["english"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Project Contributors",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "In development. Please wait.",
            ["mods_back"] = "Back",
            ["contributors_title"] = "PROJECT CONTRIBUTORS",
            ["contributors_intro"] = "Hello, I am Augusto, known as Hann.",
            ["contributors_origin"] = "About 3 months ago, I had a simple idea: \"There is no mobile port of Celeste, right?\" I searched in many places and could not find a mobile version that matched what I expected in fidelity, stability, and device experience. So I decided to create my own port.",
            ["contributors_goal"] = "From the beginning, the goal was to build a functional and stable port focused on delivering a consistent mobile experience. During the process, I faced adaptation challenges, adjustments, testing, and refinements, but with dedication and persistence I made the project work.",
            ["contributors_thanks"] = "I sincerely thank everyone who supported me and did not let me give up during development. Their support was essential to keep the project moving forward, especially in the hardest moments.",
            ["contributors_special_thanks_title"] = "SPECIAL THANKS",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nAmong others",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "I also invite anyone who wants to follow updates, news, announcements, and the continuous development of the port.",
            ["contributors_discord_mads_label"] = "Mads Studios server (port developer)",
            ["contributors_discord_partner_label"] = "Hollow Abys server (partner server)",
            ["contributors_discord_button_mads"] = "Join Mads Studios server",
            ["contributors_discord_button_partner"] = "Join Hollow Abys server",
            ["contributors_back"] = "Back"
        },
        ["brazilian"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Contribuidores do Projeto",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "Aba em desenvolvimento. Aguarde.",
            ["mods_back"] = "Voltar",
            ["contributors_title"] = "CONTRIBUIDORES DO PROJETO",
            ["contributors_intro"] = "Ola, sou o Augusto, conhecido como Hann.",
            ["contributors_origin"] = "Ha cerca de 3 meses, eu tive uma ideia simples: \"Nao existe um port mobile de Celeste, ne?\" Eu procurei em varios lugares e nao encontrei nenhuma versao mobile que atendesse ao que eu imaginava em termos de fidelidade, estabilidade e experiencia no dispositivo. Diante disso, decidi criar o meu proprio port.",
            ["contributors_goal"] = "Desde o inicio, o objetivo foi desenvolver um port funcional e estavel, com foco em oferecer uma experiencia consistente no mobile. Ao longo do processo, enfrentei desafios de adaptacao, ajustes, testes e refinamentos, mas com dedicacao e persistencia consegui colocar o projeto em funcionamento.",
            ["contributors_thanks"] = "Quero agradecer sinceramente a todos que me apoiaram e nao deixaram eu desistir durante o desenvolvimento. O suporte dessas pessoas foi essencial para manter o projeto avancando, principalmente nos momentos mais dificeis.",
            ["contributors_special_thanks_title"] = "AGRADECIMENTOS ESPECIAIS",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nEntre outros",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Tambem deixo um convite para quem deseja acompanhar as proximas atualizacoes, novidades, comunicados e o desenvolvimento continuo do port.",
            ["contributors_discord_mads_label"] = "Servidor Mads Studios (desenvolvedora do port)",
            ["contributors_discord_partner_label"] = "Servidor Hollow Abys (servidor parceiro)",
            ["contributors_discord_button_mads"] = "Entrar no servidor Mads Studios",
            ["contributors_discord_button_partner"] = "Entrar no servidor Hollow Abys",
            ["contributors_back"] = "Voltar"
        },
        ["spanish"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Colaboradores del proyecto",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "En desarrollo. Espera un momento.",
            ["mods_back"] = "Volver",
            ["contributors_title"] = "COLABORADORES DEL PROYECTO",
            ["contributors_intro"] = "Hola, soy Augusto, conocido como Hann.",
            ["contributors_origin"] = "Hace unos 3 meses tuve una idea simple: \"No existe un port movil de Celeste, verdad?\" Busque en muchos lugares y no encontre ninguna version movil que cumpliera lo que imaginaba en fidelidad, estabilidad y experiencia en el dispositivo. Por eso decidi crear mi propio port.",
            ["contributors_goal"] = "Desde el inicio, el objetivo fue desarrollar un port funcional y estable, con enfoque en ofrecer una experiencia consistente en movil. Durante el proceso enfrente desafios de adaptacion, ajustes, pruebas y refinamientos, pero con dedicacion y persistencia logre poner el proyecto en funcionamiento.",
            ["contributors_thanks"] = "Quiero agradecer sinceramente a todos los que me apoyaron y no me dejaron rendirme durante el desarrollo. Su apoyo fue esencial para mantener el proyecto avanzando, especialmente en los momentos mas dificiles.",
            ["contributors_special_thanks_title"] = "AGRADECIMIENTOS ESPECIALES",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nEntre otros",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Tambien dejo una invitacion para quien quiera seguir las proximas actualizaciones, novedades, comunicados y el desarrollo continuo del port.",
            ["contributors_discord_mads_label"] = "Servidor de Mads Studios (desarrolladora del port)",
            ["contributors_discord_partner_label"] = "Servidor Hollow Abys (servidor socio)",
            ["contributors_discord_button_mads"] = "Entrar al servidor Mads Studios",
            ["contributors_discord_button_partner"] = "Entrar al servidor Hollow Abys",
            ["contributors_back"] = "Volver"
        },
        ["french"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Contributeurs du projet",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "En developpement. Veuillez patienter.",
            ["mods_back"] = "Retour",
            ["contributors_title"] = "CONTRIBUTEURS DU PROJET",
            ["contributors_intro"] = "Bonjour, je suis Augusto, connu sous le nom de Hann.",
            ["contributors_origin"] = "Il y a environ 3 mois, j'ai eu une idee simple : \"Il n'existe pas de port mobile de Celeste, non ?\" J'ai cherche a plusieurs endroits et je n'ai trouve aucune version mobile qui corresponde a ce que j'imaginais en fidelite, stabilite et experience sur appareil. J'ai donc decide de creer mon propre port.",
            ["contributors_goal"] = "Des le depart, l'objectif etait de developper un port fonctionnel et stable, avec l'accent sur une experience mobile coherente. Pendant le processus, j'ai affronte des defis d'adaptation, des ajustements, des tests et des raffinements, mais avec devouement et perseverance, j'ai reussi a faire fonctionner le projet.",
            ["contributors_thanks"] = "Je remercie sincerement toutes les personnes qui m'ont soutenu et ne m'ont pas laisse abandonner pendant le developpement. Leur soutien a ete essentiel pour faire avancer le projet, surtout dans les moments les plus difficiles.",
            ["contributors_special_thanks_title"] = "REMERCIEMENTS SPECIAUX",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nEntre autres",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Je laisse aussi une invitation a celles et ceux qui souhaitent suivre les prochaines mises a jour, nouveautes, annonces et le developpement continu du port.",
            ["contributors_discord_mads_label"] = "Serveur Mads Studios (developpeur du port)",
            ["contributors_discord_partner_label"] = "Serveur Hollow Abys (serveur partenaire)",
            ["contributors_discord_button_mads"] = "Rejoindre le serveur Mads Studios",
            ["contributors_discord_button_partner"] = "Rejoindre le serveur Hollow Abys",
            ["contributors_back"] = "Retour"
        },
        ["german"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Projekt-Mitwirkende",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "In Entwicklung. Bitte warten.",
            ["mods_back"] = "Zuruck",
            ["contributors_title"] = "PROJEKT-MITWIRKENDE",
            ["contributors_intro"] = "Hallo, ich bin Augusto, bekannt als Hann.",
            ["contributors_origin"] = "Vor etwa 3 Monaten hatte ich eine einfache Idee: \"Es gibt keinen mobilen Port von Celeste, oder?\" Ich habe an vielen Orten gesucht und keine mobile Version gefunden, die meinen Erwartungen an Genauigkeit, Stabilitat und Gerateerlebnis entsprach. Deshalb habe ich entschieden, meinen eigenen Port zu entwickeln.",
            ["contributors_goal"] = "Von Anfang an war das Ziel, einen funktionalen und stabilen Port zu entwickeln, mit Fokus auf ein konsistentes mobiles Spielerlebnis. Im Laufe des Prozesses gab es Herausforderungen bei Anpassungen, Tests und Feinschliff, aber mit Hingabe und Ausdauer konnte ich das Projekt zum Laufen bringen.",
            ["contributors_thanks"] = "Ich danke aufrichtig allen, die mich unterstutzt und mich wahrend der Entwicklung nicht aufgeben lassen haben. Diese Unterstutzung war entscheidend, damit das Projekt weiter vorankommt, besonders in den schwierigsten Momenten.",
            ["contributors_special_thanks_title"] = "BESONDERER DANK",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nUnter anderem",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Ich lade auch alle ein, die die nachsten Updates, Neuigkeiten, Mitteilungen und die fortlaufende Entwicklung des Ports verfolgen mochten.",
            ["contributors_discord_mads_label"] = "Mads Studios Server (Port-Entwickler)",
            ["contributors_discord_partner_label"] = "Hollow Abys Server (Partner-Server)",
            ["contributors_discord_button_mads"] = "Mads Studios Server beitreten",
            ["contributors_discord_button_partner"] = "Hollow Abys Server beitreten",
            ["contributors_back"] = "Zuruck"
        },
        ["italian"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Mods",
            ["menu_project_contributors"] = "Collaboratori del progetto",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "In sviluppo. Attendere.",
            ["mods_back"] = "Indietro",
            ["contributors_title"] = "COLLABORATORI DEL PROGETTO",
            ["contributors_intro"] = "Ciao, sono Augusto, conosciuto come Hann.",
            ["contributors_origin"] = "Circa 3 mesi fa ho avuto un'idea semplice: \"Non esiste una versione mobile di Celeste, giusto?\" Ho cercato in molti posti e non ho trovato nessuna versione mobile che corrispondesse a cio che immaginavo in termini di fedelta, stabilita ed esperienza sul dispositivo. Per questo ho deciso di creare il mio port.",
            ["contributors_goal"] = "Fin dall'inizio, l'obiettivo era sviluppare un port funzionale e stabile, con attenzione a offrire un'esperienza coerente su mobile. Durante il processo ho affrontato sfide di adattamento, regolazioni, test e rifiniture, ma con dedizione e perseveranza sono riuscito a far funzionare il progetto.",
            ["contributors_thanks"] = "Voglio ringraziare sinceramente tutti coloro che mi hanno sostenuto e non mi hanno lasciato mollare durante lo sviluppo. Il supporto di queste persone e stato essenziale per far avanzare il progetto, soprattutto nei momenti piu difficili.",
            ["contributors_special_thanks_title"] = "RINGRAZIAMENTI SPECIALI",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nTra gli altri",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Lascio anche un invito a chi desidera seguire i prossimi aggiornamenti, novita, comunicati e lo sviluppo continuo del port.",
            ["contributors_discord_mads_label"] = "Server Mads Studios (sviluppatrice del port)",
            ["contributors_discord_partner_label"] = "Server Hollow Abys (server partner)",
            ["contributors_discord_button_mads"] = "Entra nel server Mads Studios",
            ["contributors_discord_button_partner"] = "Entra nel server Hollow Abys",
            ["contributors_back"] = "Indietro"
        },
        ["japanese"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "MODS",
            ["menu_project_contributors"] = "プロジェクト貢献者",
            ["mods_title"] = "MODS",
            [ModsStatusDevKey] = "開発中です。しばらくお待ちください。",
            ["mods_back"] = "戻る",
            ["contributors_title"] = "プロジェクト貢献者",
            ["contributors_intro"] = "こんにちは、私はAugusto、Hannとして知られています。",
            ["contributors_origin"] = "約3か月前、私はシンプルな考えを持ちました。\"Celesteのモバイル移植って無いよね？\" と。いろいろ探しましたが、忠実さ・安定性・端末での体験という点で理想に合うモバイル版は見つかりませんでした。そこで自分で移植を作ることにしました。",
            ["contributors_goal"] = "最初からの目標は、モバイルで一貫した体験を提供できる、機能的で安定した移植を作ることでした。開発の過程では適応・調整・テスト・改善の課題がありましたが、努力と粘り強さでプロジェクトを動かせるようになりました。",
            ["contributors_thanks"] = "開発中に私を支え、諦めさせなかった皆さんに心から感謝します。特に難しい時期に、この支えがプロジェクトを前に進める大きな力になりました。",
            ["contributors_special_thanks_title"] = "特別感謝",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nそのほかの皆さん",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "今後のアップデート、新情報、お知らせ、継続開発を追いたい方は、以下のサーバーに参加してください。",
            ["contributors_discord_mads_label"] = "Mads Studiosサーバー（移植開発チーム）",
            ["contributors_discord_partner_label"] = "Hollow Abysサーバー（パートナーサーバー）",
            ["contributors_discord_button_mads"] = "Mads Studiosサーバーに参加",
            ["contributors_discord_button_partner"] = "Hollow Abysサーバーに参加",
            ["contributors_back"] = "戻る"
        },
        ["korean"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "모드",
            ["menu_project_contributors"] = "프로젝트 기여자",
            ["mods_title"] = "모드",
            [ModsStatusDevKey] = "개발 중입니다. 잠시만 기다려 주세요.",
            ["mods_back"] = "뒤로",
            ["contributors_title"] = "프로젝트 기여자",
            ["contributors_intro"] = "안녕하세요, 저는 Augusto이며 Hann이라는 이름으로 알려져 있습니다.",
            ["contributors_origin"] = "약 3개월 전, 저는 단순한 생각을 했습니다. \"Celeste 모바일 포트가 없네?\" 여러 곳을 찾아봤지만 제가 기대하던 완성도, 안정성, 기기 경험을 만족하는 모바일 버전을 찾지 못했습니다. 그래서 직접 포트를 만들기로 했습니다.",
            ["contributors_goal"] = "처음부터 목표는 모바일에서 일관된 경험을 제공하는 기능적이고 안정적인 포트를 만드는 것이었습니다. 개발 과정에서 적응, 조정, 테스트, 개선의 어려움이 있었지만, 헌신과 끈기로 프로젝트를 실제로 동작하게 만들었습니다.",
            ["contributors_thanks"] = "개발 중에 저를 응원해 주고 포기하지 않게 해 준 모든 분들께 진심으로 감사드립니다. 특히 가장 힘든 순간에 여러분의 지원은 프로젝트를 계속 전진시키는 데 큰 힘이 되었습니다.",
            ["contributors_special_thanks_title"] = "특별 감사",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\n그 외 많은 분들",
            ["contributors_discord_title"] = "디스코드",
            ["contributors_invite"] = "포트의 다음 업데이트, 소식, 공지, 지속적인 개발 과정을 확인하고 싶은 분들을 초대합니다.",
            ["contributors_discord_mads_label"] = "Mads Studios 서버 (포트 개발팀)",
            ["contributors_discord_partner_label"] = "Hollow Abys 서버 (파트너 서버)",
            ["contributors_discord_button_mads"] = "Mads Studios 서버 참가",
            ["contributors_discord_button_partner"] = "Hollow Abys 서버 참가",
            ["contributors_back"] = "뒤로"
        },
        ["russian"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "Моды",
            ["menu_project_contributors"] = "Участники проекта",
            ["mods_title"] = "МОДЫ",
            [ModsStatusDevKey] = "В разработке. Пожалуйста, подождите.",
            ["mods_back"] = "Назад",
            ["contributors_title"] = "УЧАСТНИКИ ПРОЕКТА",
            ["contributors_intro"] = "Привет, я Аугусто, также известный как Hann.",
            ["contributors_origin"] = "Около 3 месяцев назад у меня появилась простая идея: \"Мобильного порта Celeste ведь нет?\" Я искал во многих местах и не нашел мобильной версии, которая соответствовала бы моим ожиданиям по точности, стабильности и опыту на устройстве. Поэтому я решил создать собственный порт.",
            ["contributors_goal"] = "С самого начала цель была создать функциональный и стабильный порт, ориентированный на целостный мобильный опыт. В процессе я столкнулся с задачами адаптации, настройками, тестами и доработками, но благодаря упорству и настойчивости смог запустить проект.",
            ["contributors_thanks"] = "Я искренне благодарю всех, кто поддерживал меня и не дал мне сдаться во время разработки. Эта поддержка была крайне важна для продвижения проекта, особенно в самые сложные моменты.",
            ["contributors_special_thanks_title"] = "ОСОБАЯ БЛАГОДАРНОСТЬ",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\nИ многие другие",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "Также приглашаю всех, кто хочет следить за будущими обновлениями, новостями, объявлениями и дальнейшей разработкой порта.",
            ["contributors_discord_mads_label"] = "Сервер Mads Studios (разработчик порта)",
            ["contributors_discord_partner_label"] = "Сервер Hollow Abys (партнерский сервер)",
            ["contributors_discord_button_mads"] = "Войти на сервер Mads Studios",
            ["contributors_discord_button_partner"] = "Войти на сервер Hollow Abys",
            ["contributors_back"] = "Назад"
        },
        ["schinese"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["menu_mods"] = "模组",
            ["menu_project_contributors"] = "项目贡献者",
            ["mods_title"] = "模组",
            [ModsStatusDevKey] = "开发中，请稍候。",
            ["mods_back"] = "返回",
            ["contributors_title"] = "项目贡献者",
            ["contributors_intro"] = "你好，我是 Augusto，也被称为 Hann。",
            ["contributors_origin"] = "大约 3 个月前，我有了一个简单的想法：\"Celeste 没有手机版移植，对吧？\" 我在很多地方搜索，但没有找到在还原度、稳定性和设备体验上符合我期望的移动版本。于是我决定自己制作这个移植。",
            ["contributors_goal"] = "从一开始，目标就是开发一个功能完整且稳定的移植版本，重点是提供一致的移动端体验。在过程中我遇到了适配、调整、测试和打磨等挑战，但凭借投入和坚持，我最终让项目运行了起来。",
            ["contributors_thanks"] = "我真诚感谢所有支持我、没有让我在开发中放弃的人。正是这些支持，尤其是在最困难的时候，让项目得以持续推进。",
            ["contributors_special_thanks_title"] = "特别鸣谢",
            ["contributors_special_thanks_list"] = "NEV\nWess\nKkilmi\nFeh O Careca\nNone\nTavv\n以及其他朋友",
            ["contributors_discord_title"] = "DISCORD",
            ["contributors_invite"] = "也欢迎想要关注后续更新、新闻、公告以及持续开发进度的朋友加入下方服务器。",
            ["contributors_discord_mads_label"] = "Mads Studios 服务器（移植开发团队）",
            ["contributors_discord_partner_label"] = "Hollow Abys 服务器（合作服务器）",
            ["contributors_discord_button_mads"] = "加入 Mads Studios 服务器",
            ["contributors_discord_button_partner"] = "加入 Hollow Abys 服务器",
            ["contributors_back"] = "返回"
        }
    };

    public static string Get(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        string language = ResolveLanguage();
        if (TryGet(language, key, out string value))
        {
            return value;
        }

        if (TryGet("english", key, out value))
        {
            return value;
        }

        return key;
    }

    private static bool TryGet(string language, string key, out string value)
    {
        value = string.Empty;
        if (string.IsNullOrWhiteSpace(language))
        {
            return false;
        }

        if (!TextByLanguage.TryGetValue(language, out Dictionary<string, string> map))
        {
            return false;
        }

        return map.TryGetValue(key, out value);
    }

    private static string ResolveLanguage()
    {
        string language = Dialog.Language?.Id;
        if (string.IsNullOrWhiteSpace(language))
        {
            language = Settings.Instance?.Language;
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            return "english";
        }

        return language.Trim().ToLowerInvariant();
    }
}
