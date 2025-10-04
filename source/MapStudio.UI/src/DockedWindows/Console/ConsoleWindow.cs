using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ImGuiNET;
using MapStudio.UI;
using Toolbox.Core;
using UIFramework;

namespace MapStudio.UI
{
    public class ConsoleWindow : DockWindow
    {
        public override string Name => "CONSOLE";

        bool displayErrors = true;
        bool displayWarnings = true;
        bool displayInfo = true;

        public ConsoleWindow(DockSpaceWindow parent) : base(parent)
        {

        }

        public override void Render()
        {
            ImGui.Checkbox(TranslationSource.GetText("ERRORS"), ref displayErrors); ImGui.SameLine();
            ImGui.Checkbox(TranslationSource.GetText("WARNINGS"), ref displayWarnings); ImGui.SameLine();
            ImGui.Checkbox(TranslationSource.GetText("MESSAGES"), ref displayInfo); ImGui.SameLine();
            if (ImGui.Button(TranslationSource.GetText("COPY")))
            {
                string text = "";
                if (displayErrors) text += StudioLogger.GetErrorLog();
                if (displayWarnings) text += StudioLogger.GetWarningLog();
                if (displayInfo) text += StudioLogger.GetLog();

                ImGui.SetClipboardText(text);
            }
            ImGui.SameLine();
            if (ImGui.Button("Check Errors"))
            {
                StudioLogger.ResetErrors();
                Workspace.ActiveWorkspace.PrintErrors();
            }

            var color = ImGui.GetStyle().Colors[(int)ImGuiCol.FrameBg];
            ImGui.PushStyleColor(ImGuiCol.ChildBg, color);

            ImGui.BeginChild("consoleWindow");

            // Add in transform info
            var info = GLFrameworkEngine.GLContext.ActiveContext.TransformTools.GetTextInput();
            if (!string.IsNullOrEmpty(info))
                WriteText(info, ThemeHandler.Theme.Text);

            // Write logs line by line with color
            if (displayErrors)
                WriteLogLines(StudioLogger.GetErrorLog(), LogType.Error);

            if (displayWarnings)
                WriteLogLines(StudioLogger.GetWarningLog(), LogType.Warning);

            if (displayInfo)
                WriteLogLines(StudioLogger.GetLog(), LogType.Info);

            ImGui.EndChild();

            ImGui.PopStyleColor();
        }

        enum LogType
        {
            Info,
            Warning,
            Error
        }

        private void WriteLogLines(string text, LogType defaultType)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var type = GetLogLineType(line, defaultType);
                Vector4 color = ThemeHandler.Theme.Text;
                switch (type)
                {
                    case LogType.Error:
                        color = ThemeHandler.Theme.Error;
                        break;
                    case LogType.Warning:
                        color = ThemeHandler.Theme.Warning;
                        break;
                    case LogType.Info:
                        color = ThemeHandler.Theme.Text;
                        break;
                }
                WriteText(line, color);
            }
        }

        private LogType GetLogLineType(string line, LogType defaultType)
        {
            if (line.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                return LogType.Error;
            if (line.Contains("WARN", StringComparison.OrdinalIgnoreCase))
                return LogType.Warning;
            if (line.Contains("INFO", StringComparison.OrdinalIgnoreCase))
                return LogType.Info;
            return defaultType;
        }

        private void WriteText(string text, Vector4 color)
        {
            if (string.IsNullOrEmpty(text))
                return;

            ImGui.PushStyleColor(ImGuiCol.Text, color);
            ImGui.TextWrapped(text);
            ImGui.PopStyleColor();
        }
    }
}