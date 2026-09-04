namespace MVCMiniProject.Helpers
{
    public static class EmailTemplates
    {
        public static string Verification(string platform, string fullName, string verifyUrl, int hours)
        {
            var safeName = string.IsNullOrWhiteSpace(fullName) ? "there" : fullName;
            return $@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<title>Verify your email</title>
</head>
<body style=""margin:0;padding:0;background:#f4f5fb;font-family:Arial,Helvetica,sans-serif;"">
  <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:#f4f5fb;padding:24px 12px;"">
    <tr>
      <td align=""center"">
        <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""max-width:560px;background:#ffffff;border-radius:12px;overflow:hidden;"">
          <tr>
            <td style=""background:#ff6600;padding:22px 28px;color:#ffffff;font-size:22px;font-weight:bold;"">{platform}</td>
          </tr>
          <tr>
            <td style=""padding:28px;color:#2c2b31;font-size:15px;line-height:1.6;"">
              <p style=""margin:0 0 12px;"">Hi {safeName},</p>
              <p style=""margin:0 0 16px;"">Welcome to {platform}. Please confirm your email address to activate your account.</p>
              <p style=""margin:0 0 24px;text-align:center;"">
                <a href=""{verifyUrl}"" style=""display:inline-block;background:#ff6600;color:#ffffff;text-decoration:none;padding:12px 22px;border-radius:6px;font-weight:bold;"">Email Adresimi Doğrula</a>
              </p>
              <p style=""margin:0 0 8px;font-size:13px;color:#6c6a74;"">If the button does not work, copy this link:</p>
              <p style=""margin:0 0 18px;word-break:break-all;font-size:13px;""><a href=""{verifyUrl}"" style=""color:#ff6600;"">{verifyUrl}</a></p>
              <p style=""margin:0 0 8px;font-size:13px;color:#6c6a74;"">This link expires in {hours} hours.</p>
              <p style=""margin:0;font-size:12px;color:#9aa0b5;"">If you did not create this account, you can ignore this email.</p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }
    }
}
