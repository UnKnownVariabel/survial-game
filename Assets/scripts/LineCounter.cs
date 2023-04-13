using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LineCounter : MonoBehaviour
{
    void Start()
    {
        Int32 lines = 0;
        Int32 numChars = 0;
        Int32 numFiles = 0;
        ProcessFiles("C:\\Users\\jonathan\\Documents\\GitHub\\survial-game\\Assets\\scripts", ref lines, ref numChars, ref numFiles);

        Debug.Log("Amount of cs files: " + numFiles);
        Debug.Log("Total lines: " + lines);
        Debug.Log("Non-white space characters: " + numChars);
    }

    private static void ProcessFiles(string dir, ref Int32 numLines, ref Int32 numChars, ref Int32 numFiles)
    {
        var files = Directory.GetFiles(dir);
        foreach (var file in files)
        {
            var ext = System.IO.Path.GetExtension(file);
            if (ext == ".cs")
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                    numChars += line.Trim().Length;

                numLines += lines.Length;

                numFiles++;
            }
        }

        var dirs = Directory.GetDirectories(dir);
        foreach (var d in dirs)
            ProcessFiles(d, ref numLines, ref numChars, ref numFiles);
    }
}
