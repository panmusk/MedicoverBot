using MedicoverBot.DataModel;

namespace MedicoverBot.Notifiers
{
    internal interface INotifier
    {
        public void Notify(Item appointment);
    }
}