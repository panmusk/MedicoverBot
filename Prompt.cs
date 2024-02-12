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
    }
}