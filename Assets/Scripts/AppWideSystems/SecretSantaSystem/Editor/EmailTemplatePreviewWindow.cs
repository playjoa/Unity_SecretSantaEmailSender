using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AppWideSystems.SecretSantaSystem.Data;
using AppWideSystems.SecretSantaSystem.Util;
using UnityEditor;
using UnityEngine;

namespace AppWideSystems.SecretSantaSystem
{
    public class EmailTemplatePreviewWindow : EditorWindow
    {
        private const string MenuPath = "DevTools/Secret Santa/Preview Email Template";
        private const string PreviewFolderName = "SecretSantaEmailPreview";
        private const string DefaultRecipientName = "Maria";
        private const string DefaultMatchName = "João";
        private const string DefaultMatchEmail = "joao@email.com";
        private const string DefaultMatchPhone = "+55 11 99999-0000";

        private const string PrefTemplate = "SecretSanta.EmailPreview.TemplateGuid";
        private const string PrefRecipientUser = "SecretSanta.EmailPreview.RecipientUserGuid";
        private const string PrefMatchUser = "SecretSanta.EmailPreview.MatchUserGuid";
        private const string PrefRecipientName = "SecretSanta.EmailPreview.RecipientName";
        private const string PrefMatchName = "SecretSanta.EmailPreview.MatchName";
        private const string PrefMatchEmail = "SecretSanta.EmailPreview.MatchEmail";
        private const string PrefMatchPhone = "SecretSanta.EmailPreview.MatchPhone";

        private const string PreviewNote =
            "This fills the same tokens Notify sends, writes a temporary HTML file, and opens it in the browser. Some inboxes drop gradients and fonts the browser still shows.";

        private EmailTemplateData selectedTemplate;
        private SecretSantaUserData recipientUser;
        private SecretSantaUserData matchUser;
        private string recipientName = DefaultRecipientName;
        private string matchName = DefaultMatchName;
        private string matchEmail = DefaultMatchEmail;
        private string matchPhone = DefaultMatchPhone;
        private bool showFilledHtml;
        private Vector2 filledHtmlScroll;
        private GUIStyle filledHtmlStyle;

        private bool reapplyFields;
        private bool clearUserAssetsOnReapply;
        private string reapplyRecipientName;
        private string reapplyMatchName;
        private string reapplyMatchEmail;
        private string reapplyMatchPhone;

        [MenuItem(MenuPath)]
        private static void OpenFromMenu()
        {
            Open(null);
        }

        public static void Open(EmailTemplateData template)
        {
            var window = GetWindow<EmailTemplatePreviewWindow>("Email Preview");
            window.titleContent = new GUIContent("Email Preview");
            window.Show();
            if (template != null)
            {
                window.selectedTemplate = template;
                window.SavePrefs();
            }
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Email Preview");
            minSize = new Vector2(420f, 520f);
            LoadPrefs();
        }

        private void OnDisable()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                return;

            SavePrefs();
        }

        private void OnGUI()
        {
            ApplyQueuedFields();

            EditorGUILayout.HelpBox(PreviewNote, MessageType.Info);
            DrawTemplateSelector();
            DrawTemplateMessages();
            DrawPlaceholderFields();
            DrawUserCopyFields();

            if (GUILayout.Button("Reset"))
            {
                ClearKeyboardFocus();
                recipientName = DefaultRecipientName;
                matchName = DefaultMatchName;
                matchEmail = DefaultMatchEmail;
                matchPhone = DefaultMatchPhone;
                recipientUser = null;
                matchUser = null;
                QueueFieldReapply(true);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Subject");
            EditorGUILayout.SelectableLabel(
                SecretSantaUtils.GenerateEmailSubject(recipientName),
                EditorStyles.textField,
                GUILayout.Height(EditorGUIUtility.singleLineHeight));

            EditorGUILayout.Space();
            DrawTokenStatus(RawHtml);

            EditorGUILayout.Space();
            var canPreviewSelected = selectedTemplate != null && selectedTemplate.EmailHtmlTemplate != null;
            using (new EditorGUI.DisabledScope(!canPreviewSelected))
            {
                if (GUILayout.Button("Preview in Browser"))
                    PreviewSelected();

                if (GUILayout.Button("Copy Filled HTML"))
                    EditorGUIUtility.systemCopyBuffer = BuildFilledHtml(selectedTemplate);
            }

            if (GUILayout.Button("Preview Every Template in Browser"))
                PreviewEveryTemplate();

            EditorGUILayout.Space();
            showFilledHtml = EditorGUILayout.Foldout(showFilledHtml, "Filled HTML", true);
            if (showFilledHtml)
                DrawFilledHtml();

            if (GUI.changed)
                SavePrefs();
        }

        private void DrawTemplateSelector()
        {
            var templates = LoadAllTemplates();
            var names = new string[templates.Length + 1];
            names[0] = "None";
            var selectedIndex = 0;

            for (var i = 0; i < templates.Length; i++)
            {
                names[i + 1] = templates[i].name;
                if (templates[i] == selectedTemplate)
                    selectedIndex = i + 1;
            }

            var newIndex = EditorGUILayout.Popup("Template", selectedIndex, names);
            if (newIndex != selectedIndex)
                selectedTemplate = newIndex == 0 ? null : templates[newIndex - 1];

            selectedTemplate = (EmailTemplateData)EditorGUILayout.ObjectField(
                "Template Asset",
                selectedTemplate,
                typeof(EmailTemplateData),
                false);
        }

        private void DrawTemplateMessages()
        {
            if (selectedTemplate == null)
            {
                EditorGUILayout.HelpBox(
                    "Select an Email Template Data asset. Create one from SecretSanta/DataEmailTemplateData and assign an HTML TextAsset.",
                    MessageType.Info);
                return;
            }

            if (selectedTemplate.EmailHtmlTemplate == null)
            {
                EditorGUILayout.HelpBox(
                    "This template has no HTML file assigned.",
                    MessageType.Warning);
            }
        }

        private void DrawPlaceholderFields()
        {
            recipientName = EditorGUILayout.TextField(TokenContent("Recipient name", SecretSantaUtils.HTML_USERNAME_KEY), recipientName);
            matchName = EditorGUILayout.TextField(TokenContent("Match name", SecretSantaUtils.HTML_SECRETSANTA_USERNAME_KEY), matchName);
            matchEmail = EditorGUILayout.TextField(TokenContent("Match email", SecretSantaUtils.HTML_SECRETSANTA_EMAIL_KEY), matchEmail);
            matchPhone = EditorGUILayout.TextField(TokenContent("Match phone", SecretSantaUtils.HTML_SECRETSANTA_PHONENUMBER_KEY), matchPhone);
        }

        private void DrawUserCopyFields()
        {
            EditorGUILayout.BeginHorizontal();
            recipientUser = (SecretSantaUserData)EditorGUILayout.ObjectField(
                "Recipient user",
                recipientUser,
                typeof(SecretSantaUserData),
                false);
            using (new EditorGUI.DisabledScope(recipientUser == null))
            {
                if (GUILayout.Button("Copy recipient", GUILayout.Width(130f)))
                {
                    ClearKeyboardFocus();
                    recipientName = recipientUser.UserName ?? string.Empty;
                    QueueFieldReapply(false);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            matchUser = (SecretSantaUserData)EditorGUILayout.ObjectField(
                "Match user",
                matchUser,
                typeof(SecretSantaUserData),
                false);
            using (new EditorGUI.DisabledScope(matchUser == null))
            {
                if (GUILayout.Button("Copy match", GUILayout.Width(130f)))
                {
                    ClearKeyboardFocus();
                    matchName = matchUser.UserName ?? string.Empty;
                    matchEmail = matchUser.UserEmail ?? string.Empty;
                    matchPhone = matchUser.UserPhoneNumber ?? string.Empty;
                    QueueFieldReapply(false);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTokenStatus(string rawHtml)
        {
            EditorGUILayout.LabelField("Tokens");
            DrawTokenLine(SecretSantaUtils.HTML_USERNAME_KEY, rawHtml);
            DrawTokenLine(SecretSantaUtils.HTML_SECRETSANTA_USERNAME_KEY, rawHtml);
            DrawTokenLine(SecretSantaUtils.HTML_SECRETSANTA_EMAIL_KEY, rawHtml);
            DrawTokenLine(SecretSantaUtils.HTML_SECRETSANTA_PHONENUMBER_KEY, rawHtml);
        }

        private static void DrawTokenLine(string token, string rawHtml)
        {
            var used = !string.IsNullOrEmpty(rawHtml) && rawHtml.Contains(token);
            EditorGUILayout.LabelField(token, used ? "Used" : "Not in this template");
        }

        private void DrawFilledHtml()
        {
            var filled = BuildFilledHtml(selectedTemplate);
            var style = FilledHtmlStyle;
            filledHtmlScroll = EditorGUILayout.BeginScrollView(filledHtmlScroll, GUILayout.MinHeight(160f));
            var viewWidth = Mathf.Max(EditorGUIUtility.currentViewWidth - 40f, 100f);
            var height = Mathf.Max(style.CalcHeight(new GUIContent(filled ?? string.Empty), viewWidth), 80f);
            EditorGUILayout.SelectableLabel(filled ?? string.Empty, style, GUILayout.Height(height));
            EditorGUILayout.EndScrollView();
        }

        private string RawHtml =>
            selectedTemplate != null && selectedTemplate.EmailHtmlTemplate != null
                ? selectedTemplate.EmailHtmlTemplate.text
                : string.Empty;

        private GUIStyle FilledHtmlStyle
        {
            get
            {
                if (filledHtmlStyle == null)
                {
                    filledHtmlStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
                    {
                        wordWrap = true,
                        richText = false
                    };
                }

                return filledHtmlStyle;
            }
        }

        private void PreviewSelected()
        {
            if (selectedTemplate == null || selectedTemplate.EmailHtmlTemplate == null)
                return;

            if (!TryOpenInBrowser(selectedTemplate, BuildFilledHtml(selectedTemplate), out var error))
            {
                EditorUtility.DisplayDialog(
                    "Email Preview",
                    "Could not open the email preview.\n\n" + error,
                    "OK");
            }
        }

        private void PreviewEveryTemplate()
        {
            var templates = LoadAllTemplates();
            var opened = 0;

            for (var i = 0; i < templates.Length; i++)
            {
                var template = templates[i];
                if (template == null || template.EmailHtmlTemplate == null)
                {
                    if (template != null)
                        Debug.LogWarning($"Email preview skipped '{template.name}' because it has no HTML file assigned.", template);
                    continue;
                }

                if (TryOpenInBrowser(template, BuildFilledHtml(template), out var error))
                {
                    opened++;
                    continue;
                }

                Debug.LogWarning($"Email preview could not open '{template.name}': {error}", template);
            }

            if (opened == 0)
            {
                EditorUtility.DisplayDialog(
                    "Email Preview",
                    "No email templates with an HTML file could be opened.",
                    "OK");
            }
        }

        private string BuildFilledHtml(EmailTemplateData template)
        {
            if (template == null || template.EmailHtmlTemplate == null)
                return string.Empty;

            return SecretSantaUtils.FillEmailHtmlTemplate(
                template.EmailHtmlTemplate.text,
                recipientName,
                matchName,
                matchEmail,
                matchPhone);
        }

        private static bool TryOpenInBrowser(EmailTemplateData template, string filledHtml, out string error)
        {
            error = null;

            try
            {
                var folder = Path.Combine(Path.GetTempPath(), PreviewFolderName);
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, SanitizeFileName(template.name) + ".html");
                File.WriteAllText(path, filledHtml ?? string.Empty, new UTF8Encoding(false));
                EditorUtility.OpenWithDefaultApp(path);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static string SanitizeFileName(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName))
                return "EmailTemplate";

            var invalid = Path.GetInvalidFileNameChars();
            var builder = new StringBuilder(assetName.Length);
            for (var i = 0; i < assetName.Length; i++)
            {
                var character = assetName[i];
                builder.Append(Array.IndexOf(invalid, character) >= 0 ? '_' : character);
            }

            var sanitized = builder.ToString().Trim();
            if (string.IsNullOrEmpty(sanitized) || sanitized == "." || sanitized == "..")
                return "EmailTemplate";

            return sanitized;
        }

        private static EmailTemplateData[] LoadAllTemplates()
        {
            var guids = AssetDatabase.FindAssets("t:EmailTemplateData");
            var templates = new List<EmailTemplateData>(guids.Length);
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var template = AssetDatabase.LoadAssetAtPath<EmailTemplateData>(path);
                if (template != null)
                    templates.Add(template);
            }

            templates.Sort(CompareTemplateNames);
            return templates.ToArray();
        }

        private static int CompareTemplateNames(EmailTemplateData left, EmailTemplateData right)
        {
            return string.Compare(left.name, right.name, StringComparison.OrdinalIgnoreCase);
        }

        private static GUIContent TokenContent(string label, string token)
        {
            return new GUIContent(label, "Replaces " + token);
        }

        private void QueueFieldReapply(bool clearUserAssets)
        {
            ClearKeyboardFocus();
            reapplyRecipientName = recipientName ?? string.Empty;
            reapplyMatchName = matchName ?? string.Empty;
            reapplyMatchEmail = matchEmail ?? string.Empty;
            reapplyMatchPhone = matchPhone ?? string.Empty;
            clearUserAssetsOnReapply = clearUserAssets;
            reapplyFields = true;
            SavePrefs();
            Repaint();
        }

        private void ApplyQueuedFields()
        {
            if (!reapplyFields)
                return;

            ClearKeyboardFocus();
            recipientName = reapplyRecipientName ?? string.Empty;
            matchName = reapplyMatchName ?? string.Empty;
            matchEmail = reapplyMatchEmail ?? string.Empty;
            matchPhone = reapplyMatchPhone ?? string.Empty;
            if (clearUserAssetsOnReapply)
            {
                recipientUser = null;
                matchUser = null;
            }

            reapplyFields = false;
            clearUserAssetsOnReapply = false;
            SavePrefs();
        }

        private static void ClearKeyboardFocus()
        {
            GUI.FocusControl(null);
            GUIUtility.keyboardControl = 0;
            EditorGUIUtility.editingTextField = false;
        }

        private void LoadPrefs()
        {
            selectedTemplate = LoadAsset<EmailTemplateData>(EditorPrefs.GetString(PrefTemplate, string.Empty));
            recipientUser = LoadAsset<SecretSantaUserData>(EditorPrefs.GetString(PrefRecipientUser, string.Empty));
            matchUser = LoadAsset<SecretSantaUserData>(EditorPrefs.GetString(PrefMatchUser, string.Empty));
            recipientName = EditorPrefs.GetString(PrefRecipientName, DefaultRecipientName);
            matchName = EditorPrefs.GetString(PrefMatchName, DefaultMatchName);
            matchEmail = EditorPrefs.GetString(PrefMatchEmail, DefaultMatchEmail);
            matchPhone = EditorPrefs.GetString(PrefMatchPhone, DefaultMatchPhone);
        }

        private void SavePrefs()
        {
            EditorPrefs.SetString(PrefTemplate, AssetGuid(selectedTemplate));
            EditorPrefs.SetString(PrefRecipientUser, AssetGuid(recipientUser));
            EditorPrefs.SetString(PrefMatchUser, AssetGuid(matchUser));
            EditorPrefs.SetString(PrefRecipientName, recipientName ?? string.Empty);
            EditorPrefs.SetString(PrefMatchName, matchName ?? string.Empty);
            EditorPrefs.SetString(PrefMatchEmail, matchEmail ?? string.Empty);
            EditorPrefs.SetString(PrefMatchPhone, matchPhone ?? string.Empty);
        }

        private static string AssetGuid(UnityEngine.Object asset)
        {
            if (asset == null)
                return string.Empty;

            var path = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            return AssetDatabase.AssetPathToGUID(path);
        }

        private static T LoadAsset<T>(string guid) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(guid))
                return null;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
                return null;

            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }

    [CustomEditor(typeof(EmailTemplateData))]
    public class EmailTemplateDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Preview Email"))
                EmailTemplatePreviewWindow.Open((EmailTemplateData)target);
        }
    }
}
