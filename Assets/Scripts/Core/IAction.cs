namespace  RPG.Core
{
    public interface IAction
    {
        void Cancel();

        bool GetDoesCancel();
    }        
}