using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EmailSending : MonoBehaviour
{
    //Variables will be given when user inputs proper fields
    public InputField bodyMessage;
    public InputField userEmail;
    public Text name;
    public InputField nameInput;
    public void SendEmail()
    {
        //Creating new Mail message
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;
        
        //Adding from and to emails
        mail.From = new MailAddress("vcit.health@gmail.com");
        mail.To.Add(new MailAddress(userEmail.text));

        //Adding subject and body text
        //Body text can be added here within script, but showing that it can be added within unity
        mail.Subject = "Testing Subject";
        mail.Body = bodyMessage.text;

        //Adding the file to the email
        mail.Attachments.Add(new Attachment("Assets/Images/Screenshots/Certificate_VCIT.png"));

        //Verifying login
        SmtpServer.Credentials = new System.Net.NetworkCredential("vcit.health@gmail.com", "VCIT1234") as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        //Creates an Error notification if delivery failed, and sends otherwise.
        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
        SmtpServer.Send(mail);
    }

    IEnumerator SnapshotThenEmail()
    {
        //Setting the name for the Certificate
        name.text = nameInput.text;
        
        //Taking screenshot of certificate
        CertSnap.TakeScreenshot_Static(1024, 780);
        yield return new WaitForEndOfFrame();
        SendEmail();
    }

    public void Clicked()
    {
        StartCoroutine(SnapshotThenEmail());
    }
}
