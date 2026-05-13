namespace SUPPLIERS.Controller
{
    class GeneralController
    {
        public static string GetName(string Name) => Name.Substring(Name.IndexOf('_') + 1);

        public static int NewIndexComboBox(int NewCount, int OldCount, int Index) =>
            OldCount > NewCount ? NewCount - 1 : Index;
    }
}
