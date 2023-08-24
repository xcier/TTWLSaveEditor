using Microsoft.Win32;
namespace IOTools
{
	public class Dialogs
	{
		public static string OpenFileDialog(string Title = "", string Filter = "All Files (*.*)|*.*", string InitialDirectory = "")
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Title;
			openFileDialog.Filter = Filter;
			openFileDialog.Multiselect = false;
			openFileDialog.InitialDirectory = InitialDirectory;
			if (openFileDialog.ShowDialog() == true) return openFileDialog.FileName;
			return null;
		}
		public static string[] OpenMultiFileDialog(string Title = "", string Filter = "All Files (*.*)|*.*", string InitialDirectory = "")
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Title;
			openFileDialog.Filter = Filter;
			openFileDialog.Multiselect = true;
			openFileDialog.InitialDirectory = InitialDirectory;
			if (openFileDialog.ShowDialog() == true) return openFileDialog.FileNames;
			return null;
		}
		public static string SaveFileDialog(string Title = "", string Filter = "", string InitialDirectory = "", bool ShowOverwritePrompt = true, string AutoAddExtention = null)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = Title;
			saveFileDialog.Filter = Filter;
			saveFileDialog.InitialDirectory = InitialDirectory;
			saveFileDialog.OverwritePrompt = ShowOverwritePrompt;
			if (AutoAddExtention != null) saveFileDialog.AddExtension = true;
			saveFileDialog.DefaultExt = AutoAddExtention;
			if (saveFileDialog.ShowDialog() == true) return saveFileDialog.FileName;
			return null;
		}
	}
}
