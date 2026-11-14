# Unity Secret Santa Email Sender

Editor tool for drawing a Secret Santa and emailing each person their match. You create the group as ScriptableObjects, press Play, draw the pairs, and send one HTML email per person through Gmail SMTP.

Built with Unity 6.6 (`6000.6.0f1`).

## Requirements

- Unity 6.6 (`6000.6.0f1`), or another Unity 6 editor that can open this project
- A Gmail account with 2-Step Verification and an [App Password](https://myaccount.google.com/apppasswords)
- At least two participants

## How a draw works

Each person is a `SecretSantaUser` asset with a name, email, and phone number. Those assets sit on a `SecretSantaCandidatesData` list. **Draw Secret Santa** shuffles that list until nobody is assigned themselves, retrying up to 100 times, and writes the pairs onto a `SecretSantaResultData` asset.

**Notify Users** sends one HTML email to each person who is not already marked as notified, then marks them notified. **Reset e-mail notifications** clears those flags so a later notify can send again. Mail that already went out stays sent.

Play starts in `Assets/Scenes/_StartUp.unity`, which loads `Assets/Scenes/Main.unity`. The three buttons are on that main screen. `SecretSantaController` on the startup scene holds the candidates list, the result asset, and the email template. `EmailSystemController` sends through SMTP.

## Set up Gmail

Sending uses `smtp.gmail.com` on port 587 with SSL. That is already set in `EmailSystemController`. Gmail rejects a normal account password for this, so create an App Password:

1. Turn on 2-Step Verification for the Google account that will send the mail.
2. Open [App Passwords](https://myaccount.google.com/apppasswords) and create one for Mail.
3. Copy the 16-character password. Spaces in the password do not matter.

Open `Assets/Data/Email/EmailProviderData.asset` and set:

- **Email Provider:** `smtp.gmail.com`
- **Admin Email:** the Gmail address that sends the messages
- **Admin Password:** the App Password

The password is stored in plain text on that asset. When Play mode starts, the Console prints the provider, the address, and the password. Use a dedicated Gmail account when you can, replace the sample values before a real send, and keep a real App Password out of git. The asset in the repo uses placeholders (`myemail@gmail.com` / `myemailPassword`).

Another provider works if it accepts SMTP on port 587 with SSL. Put its host in **Email Provider** and the account in the same two fields. The port and SSL flag stay in `EmailSystemController`.

## Create the group

Sample users, a sample pool, and a sample result already live under `Assets/Data/SecretSanta/`, so you can try a draw before entering real people. Swap in your own group before you send mail.

Create assets from the Project window with **Create**:

| Asset | Menu path |
| --- | --- |
| Secret Santa user | `SecretSanta/DataSecretSantaUserData` |
| Candidates list | `SecretSanta/DataSecretSantaCandidatesData` |
| Draw result | `SecretSanta/DataSecretSantaResultData` |
| Email template | `SecretSanta/DataEmailTemplateData` |
| SMTP account | `E-mail System/DataEmailProviderData` |

The menu paths are joined that way in code (`SecretSanta/Data` plus the type name, and `E-mail System/Data` plus the type name).

On each user, fill **User Name**, **User Email**, and **User Phone Number**. Add every user to the candidates list. The draw stops if the list has fewer than two people.

In `Assets/Scenes/_StartUp.unity`, select `SecretSantaController` and assign:

- **Email Template Data** — which HTML email to send
- **Current Secret Santa Candidates** — the group to draw
- **Current Secret Santa Result Data** — where the pairs are saved

The scene already points at the test assets:

- `Assets/Data/Email/TestEmailTemplateData.asset`
- `Assets/Data/SecretSanta/CandidatesPool/_Test/SecretSantaCandidates Test.asset`
- `Assets/Data/SecretSanta/Results/_Test/SecretSantaResult Test.asset`

## Draw and send

1. Open `_StartUp` and press Play. The main scene loads on its own.
2. Click **Draw Secret Santa**. The Console logs success, or an error if the list is too short or a valid draw cannot be found. In the editor, the result asset is saved right away. You can inspect the pairs on that asset. Anyone with the result asset can see every match, so treat that file as private.
3. Click **Notify Users**. Each person who is not already marked notified gets one email, then their result is marked notified. People already marked are skipped, so a second click does not send them another copy.
4. Click **Reset e-mail notifications** when those people should be eligible for another send.

Watch the Console for `Email Sent to ...` or an SMTP error. A rejected App Password or a bad recipient address shows up there.

## Email copy

The HTML file on the assigned `EmailTemplateData` is the message body. Notify replaces these tokens in `FillEmailHtmlTemplate` before the send. The preview window calls that same method:

| Token | Replaced with |
| --- | --- |
| `{UserName}` | The recipient's name |
| `{SecretSantaName}` | The person they drew |
| `{SecretSantaEmail}` | That person's email |
| `{SecretSantaPhone}` | That person's phone |

The subject is set in code, in `GenerateEmailSubject` inside `Assets/Scripts/AppWideSystems/SecretSantaSystem/Util/SecretSantaUtils.cs`. The default is `Your Secret Santa draw - {recipient name}`. Edit that string for your group, year, or language. Messages are sent from the address in `EmailProviderData`.

## Preview an email

You can fill a template and open it in the browser without sending mail. The window uses `GenerateEmailSubject` for the subject and `FillEmailHtmlTemplate` for the body, the same path **Notify Users** uses. The subject is shown in the window and is not written into the HTML.

In the menu bar, choose **DevTools > Secret Santa > Preview Email Template**. On an `EmailTemplateData` asset, **Preview Email** opens the same window with that asset selected.

**Preview in Browser** writes a UTF-8 HTML file (no BOM) under a temporary `SecretSantaEmailPreview` folder and opens it with the default application. **Preview Every Template in Browser** does that for each `EmailTemplateData` that has an HTML file, and skips the rest. Nothing is sent, and `EmailProviderData` is not read. Some inboxes drop gradients and fonts that the browser still shows.

## Choose a template

HTML files live in `Assets/Data/HTML Templates/`:

- `Email_SecretSanta2024_HtmlTemplate.html` — Portuguese Amigo Secreto, red header. `TestEmailTemplateData` uses this file. The file name stays as it is so that asset keeps its link.
- `Email_SecretSanta_Classic_EN.html` — English Christmas card, red and green.
- `Email_SecretSanta_Minimal_EN.html` — short English note in a simple table.
- `Email_SecretSanta_NoiteNatal_PT.html` — Portuguese card, deep green and gold.

To switch layouts:

1. Create an `EmailTemplateData` asset, or duplicate `Assets/Data/Email/TestEmailTemplateData.asset`.
2. Assign one HTML file to **Email Html Template**.
3. Assign that asset to **Email Template Data** on `SecretSantaController` in `_StartUp`.

To write your own, copy one of those files and keep the four tokens spelled the same way. The file text is sent as the HTML body.

## Where things live

- `Assets/Scenes/_StartUp.unity` — boots the app and holds `SecretSantaController` and `EmailSystemController`
- `Assets/Scenes/Main.unity` — **Draw Secret Santa**, **Notify Users**, and **Reset e-mail notifications**
- `Assets/Data/SecretSanta/Users/` — one asset per person
- `Assets/Data/SecretSanta/CandidatesPool/` — the list that gets drawn
- `Assets/Data/SecretSanta/Results/` — saved pairs and notified flags
- `Assets/Data/Email/` — SMTP account and the template asset
- `Assets/Data/HTML Templates/` — email layouts
- `Assets/Scripts/AppWideSystems/SecretSantaSystem/` — draw, notify, and template tokens
- `Assets/Scripts/AppWideSystems/SecretSantaSystem/Editor/EmailTemplatePreviewWindow.cs` — editor-only email preview. It does not send mail
- `Assets/Scripts/AppWideSystems/EmailSystem/` — SMTP send
