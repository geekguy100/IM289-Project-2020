/*****************************************************************************
// File Name :         ScriptKeywordProcessor.cs
// Author :            John P. Doran
// Creation Date :     January 16, 2020
//
// Brief Description : Upon the creation of a new script, this file will 
//                     replace the keywords provided
*****************************************************************************/
using UnityEngine; // Application

internal sealed class ScriptKeywordProcessor : 
                                         UnityEditor.AssetModificationProcessor
{
    /// <summary>
    /// Unity calls this method when it is about to create an Asset hasn't been
    /// imported
    /// </summary>
    /// <param name="path">The path of the newly created file</param>
    public static void OnWillCreateAsset(string path)
    {
        // Will first check that the user is actually trying to create a
        // script
        if(IsPathToScriptFile(ref path))
        {
            // Access the contents of the file
            string fileContent = System.IO.File.ReadAllText(path);

            // Replace the following keyword with a new value
            string date = System.DateTime.Now.ToString("MMMM dd, yyyy");
            fileContent = fileContent.Replace("#CREATIONDATE#", date);

            // Write the changes to the file
            System.IO.File.WriteAllText(path, fileContent);

            // Force Unity to refresh the Asset Database with the new contents
            UnityEditor.AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Will determine if the path given is for a script (.cs) file and will
    /// alter the path so it can be read and written to
    /// </summary>
    /// <param name="path">The path of the file to be created, will be modified
    /// by this function</param>
    /// <returns>If the path is valid to work with</returns>
    private static bool IsPathToScriptFile(ref string path)
    {
        // Figure out the filetype

        // Remove the .meta from the file if there is one
        path = path.Replace(".meta", "");

        // Find the last time there's a period in the file (the filetype)
        int index = path.LastIndexOf(".");

        // If no period is found, return
        if (index == -1)
            return false;

        // Find contents at the end of the path after the last period
        string file = path.Substring(index);

        // If the filetype is not a .cs file, then it's not a script so we can 
        // quit
        if (file != ".cs")
            return false;

        // If it's a cs file, find where Assets folder is located
        index = Application.dataPath.LastIndexOf("Assets");

        // Remove everything up to the Assets folder
        path = Application.dataPath.Substring(0, index) + path;

        // If the file does not exist, don't do anything
        if (!System.IO.File.Exists(path))
            return false;

        // If the file exists, we can open it
        return true;
    }
}
