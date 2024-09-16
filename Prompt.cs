using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Spectre.Console;

namespace MedicoverBot
{
    static class Prompt
    {
        public static KeyWord SelectKeyWord(KeyWordsObject keyWords)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<KeyWord>()
                .AddChoices(keyWords.KeyWords.OrderBy(x => x.Value))
                .UseConverter(x => x.Value)
            );
        }
        public static Section SelectSection(IEnumerable<Section> sections)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Section>()
                .AddChoices(sections)
                .UseConverter(x => x.Name)
            );
        }
        public static Entry SelectEntry(IEnumerable<Entry> entries)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Entry>()
                .AddChoices(entries)
                .UseConverter(x => x.Text)
            );
        }
        public static MedicoverCredentials SelectProfile(IEnumerable<MedicoverCredentials> profiles)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<MedicoverCredentials>()
                .AddChoices(profiles)
                .UseConverter(x => x.Name)
            );
        }
    }
}