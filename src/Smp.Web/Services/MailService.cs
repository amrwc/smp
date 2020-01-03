using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Scriban;
using Smp.Web.Wrappers;

namespace Smp.Web.Services
{
    public interface IMailService
    {
        Task SendEmail(string receiverEmail, string subject, string body);
        Task SendResetPasswordEmail(string receiverEmail, string name, string resetLink);
    }

    public class MailService : IMailService
    {
        private readonly ISmtpClient _smtpClient;

        public MailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendEmail(string rcvr, string sub, string body)
        {
            var email = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(rcvr))
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(email);
        }

        public async Task SendResetPasswordEmail(string receiverEmail, string name, string resetLink)
        {
            var template = Template.Parse(EmailTemplate);
            var body = await template.RenderAsync(new { Name = name, Link = resetLink });

            var email = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(receiverEmail))
            {
                Subject = "SMP - Password Reset",
                IsBodyHtml = true
            };

            var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            var bgTop = new LinkedResource(@".\Resources\Email\bg_top.jpg") { ContentId = "bg_top" };
            var bgBottom = new LinkedResource(@".\Resources\Email\bg_bottom.jpg") { ContentId = "bg_bottom" };
            var smpLogo = new LinkedResource(@".\Resources\Email\smp-logo.png") { ContentId = "smplogo" };
            var instagram = new LinkedResource(@".\Resources\Email\instagram2x.png") { ContentId = "instagram" };
            var linkedin = new LinkedResource(@".\Resources\Email\linkedin2x.png") { ContentId = "linkedin" };
            var twitter = new LinkedResource(@".\Resources\Email\twitter2x.png") { ContentId = "twitter" };

			view.LinkedResources.Add(bgTop);
			view.LinkedResources.Add(bgBottom);
			view.LinkedResources.Add(smpLogo);
			view.LinkedResources.Add(instagram);
			view.LinkedResources.Add(linkedin);
			view.LinkedResources.Add(twitter);

			email.AlternateViews.Add(view);

            await _smtpClient.SendMailAsync(email);
        }

        private const string EmailTemplate = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>

<html xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:v='urn:schemas-microsoft-com:vml'>
<head>
<!--[if gte mso 9]><xml><o:OfficeDocumentSettings><o:AllowPNG/><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml><![endif]-->
<meta content='text/html; charset=utf-8' http-equiv='Content-Type'/>
<meta content='width=device-width' name='viewport'/>
<!--[if !mso]><!-->
<meta content='IE=edge' http-equiv='X-UA-Compatible'/>
<!--<![endif]-->
<title></title>
<!--[if !mso]><!-->
<!--<![endif]-->
<style type='text/css'>
		body {
			margin: 0;
			padding: 0;
		}

		table,
		td,
		tr {
			vertical-align: top;
			border-collapse: collapse;
		}

		* {
			line-height: inherit;
		}

		a[x-apple-data-detectors=true] {
			color: inherit !important;
			text-decoration: none !important;
		}
	</style>
<style id='media-query' type='text/css'>
		@media (max-width: 670px) {

			.block-grid,
			.col {
				min-width: 320px !important;
				max-width: 100% !important;
				display: block !important;
			}

			.block-grid {
				width: 100% !important;
			}

			.col {
				width: 100% !important;
			}

			.col>div {
				margin: 0 auto;
			}

			img.fullwidth,
			img.fullwidthOnMobile {
				max-width: 100% !important;
			}

			.no-stack .col {
				min-width: 0 !important;
				display: table-cell !important;
			}

			.no-stack.two-up .col {
				width: 50% !important;
			}

			.no-stack .col.num4 {
				width: 33% !important;
			}

			.no-stack .col.num8 {
				width: 66% !important;
			}

			.no-stack .col.num4 {
				width: 33% !important;
			}

			.no-stack .col.num3 {
				width: 25% !important;
			}

			.no-stack .col.num6 {
				width: 50% !important;
			}

			.no-stack .col.num9 {
				width: 75% !important;
			}

			.video-block {
				max-width: none !important;
			}

			.mobile_hide {
				min-height: 0px;
				max-height: 0px;
				max-width: 0px;
				display: none;
				overflow: hidden;
				font-size: 0px;
			}

			.desktop_hide {
				display: block !important;
				max-height: none !important;
			}
		}
	</style>
</head>
<body class='clean-body' style='margin: 0; padding: 0; -webkit-text-size-adjust: 100%; background-color: #F5F5F5;'>
<!--[if IE]><div class='ie-browser'><![endif]-->
<table bgcolor='#F5F5F5' cellpadding='0' cellspacing='0' class='nl-container' role='presentation' style='table-layout: fixed; vertical-align: top; min-width: 320px; Margin: 0 auto; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #F5F5F5; width: 100%;' valign='top' width='100%'>
<img width='100%' border='0' src='cid:bg_top' style='background-position:top center;background-repeat:repeat;background-color:transparent;'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td style='word-break: break-word; vertical-align: top;' valign='top'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td align='center' style='background-color:#F5F5F5'><![endif]-->
<div style='background-image:url('cid:bg_top');background-position:top center;background-repeat:repeat;background-color:transparent;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-image:url('cid:bg_top');background-position:top center;background-repeat:repeat;background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:transparent;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<table border='0' cellpadding='0' cellspacing='0' class='divider' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td class='divider_inner' style='word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' border='0' cellpadding='0' cellspacing='0' class='divider_content' height='31' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 0px solid transparent; height: 31px; width: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td height='31' style='word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top'><span></span></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:transparent;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:transparent;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 15px; padding-left: 15px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 15px; padding-left: 15px;'>
<!--<![endif]-->
<div align='left' class='img-container left fixedwidth' style='padding-right: 15px;padding-left: 10px;'>
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr style='line-height:0px'><td style='padding-right: 15px;padding-left: 10px;' align='left'><![endif]--><img alt='SMP Logo' border='0' class='left fixedwidth' src='cid:smplogo' style='text-decoration: none; -ms-interpolation-mode: bicubic; border: 0; height: auto; width: 100%; max-width: 130px; display: block;' title='SMP Logo' width='130'/>
<!--[if mso]></td></tr></table><![endif]-->
</div>
<table border='0' cellpadding='0' cellspacing='0' class='divider' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td class='divider_inner' style='word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' border='0' cellpadding='0' cellspacing='0' class='divider_content' height='10' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 0px solid transparent; height: 10px; width: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td height='10' style='word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top'><span></span></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:transparent;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: #FFFFFF;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:#FFFFFF;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:#FFFFFF'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:#FFFFFF;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:0px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:0px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 10px; padding-left: 10px; padding-top: 0px; padding-bottom: 10px; font-family: Tahoma, Verdana, sans-serif'><![endif]-->
<div style='color:#1a6eff;font-family:Tahoma, Verdana, Segoe, sans-serif;line-height:1.2;padding-top:0px;padding-right:10px;padding-bottom:10px;padding-left:10px;'>
<div style='font-family: Tahoma, Verdana, Segoe, sans-serif; line-height: 1.2; font-size: 12px; color: #1a6eff; mso-line-height-alt: 14px;'>
<p style='line-height: 1.2; font-size: 42px; word-break: break-word; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 50px; margin: 0;'><span style='font-size: 42px;'><strong><span style='font-size: 42px;'>Hello {{name}},</span></strong></span></p>
</div>
</div>
<!--[if mso]></td></tr></table><![endif]-->
<table border='0' cellpadding='0' cellspacing='0' class='divider' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td class='divider_inner' style='word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' border='0' cellpadding='0' cellspacing='0' class='divider_content' height='0' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 0px solid transparent; height: 0px; width: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td height='0' style='word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top'><span></span></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Tahoma, Verdana, sans-serif'><![endif]-->
<div style='color:#555555;font-family:Tahoma, Verdana, Segoe, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;'>
<div style='line-height: 1.2; font-size: 12px; color: #555555; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14px;'>
<p style='text-align: left; line-height: 1.2; word-break: break-word; font-size: 20px; mso-line-height-alt: 24px; margin: 0;'><span style='font-size: 20px;'>We received a request to reset your password. If this was you, click here to reset your password:</span></p>
<p style='text-align: left; line-height: 1.2; word-break: break-word; mso-line-height-alt: NaNpx; margin: 0;'> </p>
<p style='text-align: center; line-height: 1.2; word-break: break-word; mso-line-height-alt: NaNpx; margin: 0;'><a href='{{link}}' rel='noopener' style='text-decoration: underline; color: #0068A5;' target='_blank' title='Reset password'>{{link}}</a></p>
<p style='text-align: left; line-height: 1.2; word-break: break-word; mso-line-height-alt: NaNpx; margin: 0;'> </p>
<p style='text-align: left; line-height: 1.2; word-break: break-word; font-size: 20px; mso-line-height-alt: 24px; margin: 0;'><span style='font-size: 20px;'>If you did not want to reset your password, you can ignore this email.</span></p>
</div>
</div>
<!--[if mso]></td></tr></table><![endif]-->
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:transparent;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: #FFFFFF;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:#FFFFFF;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:#FFFFFF'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:#FFFFFF;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:0px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:0px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<table border='0' cellpadding='0' cellspacing='0' class='divider' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td class='divider_inner' style='word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' border='0' cellpadding='0' cellspacing='0' class='divider_content' height='0' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 0px solid transparent; height: 0px; width: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td height='0' style='word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top'><span></span></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:transparent;'>
<div class='block-grid mixed-two-up no-stack' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: #E4E9ED;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:#E4E9ED;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:#E4E9ED'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='162' style='background-color:#E4E9ED;width:162px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:0px; padding-bottom:0px;'><![endif]-->
<div class='col num3' style='display: table-cell; vertical-align: top; max-width: 320px; min-width: 162px; width: 162px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:0px; padding-bottom:0px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<div align='center' class='img-container center fixedwidth' style='padding-right: 0px;padding-left: 0px;'>
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr style='line-height:0px'><td style='padding-right: 0px;padding-left: 0px;' align='center'><![endif]--><img align='center' alt='SMP 
' border='0' class='center fixedwidth' src='cid:smplogo' style='text-decoration: none; -ms-interpolation-mode: bicubic; border: 0; height: auto; width: 100%; max-width: 121px; display: block;' title='SMP Logo' width='121'/>
<!--[if mso]></td></tr></table><![endif]-->
</div>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td><td align='center' width='487' style='background-color:#E4E9ED;width:487px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:0px; padding-bottom:0px;'><![endif]-->
<div class='col num9' style='display: table-cell; vertical-align: top; min-width: 320px; max-width: 486px; width: 487px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:0px; padding-bottom:0px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 10px; padding-left: 10px; padding-top: 20px; padding-bottom: 25px; font-family: Tahoma, Verdana, sans-serif'><![endif]-->
<div style='color:#625050;font-family:Tahoma, Verdana, Segoe, sans-serif;line-height:1.5;padding-top:20px;padding-right:10px;padding-bottom:25px;padding-left:10px;'>
<div style='font-family: Tahoma, Verdana, Segoe, sans-serif; font-size: 12px; line-height: 1.5; color: #625050; mso-line-height-alt: 18px;'>
<p style='font-size: 14px; line-height: 1.5; text-align: left; word-break: break-word; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 21px; margin: 0;'><span style='color: #1a6eff; font-size: 14px;'><strong><span style='font-size: 22px;'>See you soon,</span></strong></span></p>
<p style='font-size: 18px; line-height: 1.5; text-align: left; word-break: break-word; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 27px; margin: 0;'><span style='font-size: 18px;'><strong>SMP</strong></span></p>
</div>
</div>
<!--[if mso]></td></tr></table><![endif]-->
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:transparent;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:transparent;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<table border='0' cellpadding='0' cellspacing='0' class='divider' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td class='divider_inner' style='word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' border='0' cellpadding='0' cellspacing='0' class='divider_content' height='40' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 0px solid transparent; height: 40px; width: 100%;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td height='40' style='word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' valign='top'><span></span></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-image:url('cid:bg_bottom');background-position:top center;background-repeat:repeat;background-color:transparent;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:transparent;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:#4D8EFD;'>
<img width='100%' border='0' src='cid:bg_bottom' style='background-position:top center;background-repeat:repeat;background-color:transparent;'>
<div class='block-grid two-up no-stack' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#4D8EFD;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='325' style='background-color:transparent;width:325px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num6' style='min-width: 320px; max-width: 325px; display: table-cell; vertical-align: top; width: 325px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Tahoma, Verdana, sans-serif'><![endif]-->
<div style='color:#FFFFFF;font-family:Tahoma, Verdana, Segoe, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;'>
<div style='font-family: Tahoma, Verdana, Segoe, sans-serif; line-height: 1.2; font-size: 12px; color: #FFFFFF; mso-line-height-alt: 14px;'>
<p style='line-height: 1.2; font-size: 30px; font-family: Tahoma, Verdana, Segoe, sans-serif; word-break: break-word; mso-line-height-alt: 36px; margin: 0;'><span style='font-size: 30px;'><strong><span style='font-size: 30px;'>Stay in touch!</span></strong></span></p>
</div>
</div>
<!--[if mso]></td></tr></table><![endif]-->
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td><td align='center' width='325' style='background-color:transparent;width:325px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:5px; padding-bottom:5px;'><![endif]-->
<div class='col num6' style='min-width: 320px; max-width: 325px; display: table-cell; vertical-align: top; width: 325px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:5px; padding-bottom:5px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<table cellpadding='0' cellspacing='0' class='social_icons' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;' valign='top' width='100%'>
<tbody>
<tr style='vertical-align: top;' valign='top'>
<td style='word-break: break-word; vertical-align: top; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;' valign='top'>
<table align='center' cellpadding='0' cellspacing='0' class='social_table' role='presentation' style='table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-tspace: 0; mso-table-rspace: 0; mso-table-bspace: 0; mso-table-lspace: 0;' valign='top'>
<tbody>
<tr align='center' style='vertical-align: top; display: inline-block; text-align: center;' valign='top'>
<td style='word-break: break-word; vertical-align: top; padding-bottom: 5px; padding-right: 8px; padding-left: 8px;' valign='top'><a href='https://twitter.com/' target='_blank'><img alt='Twitter' height='32' src='cid:twitter' style='text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: none; display: block;' title='Twitter' width='32'/></a></td>
<td style='word-break: break-word; vertical-align: top; padding-bottom: 5px; padding-right: 8px; padding-left: 8px;' valign='top'><a href='https://instagram.com/' target='_blank'><img alt='Instagram' height='32' src='cid:instagram' style='text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: none; display: block;' title='Instagram' width='32'/></a></td>
<td style='word-break: break-word; vertical-align: top; padding-bottom: 5px; padding-right: 8px; padding-left: 8px;' valign='top'><a href='https://www.linkedin.com/' target='_blank'><img alt='LinkedIn' height='32' src='cid:linkedin' style='text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: none; display: block;' title='LinkedIn' width='32'/></a></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<div style='background-color:#2575FF;'>
<div class='block-grid' style='Margin: 0 auto; min-width: 320px; max-width: 650px; overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;'>
<div style='border-collapse: collapse;display: table;width: 100%;background-color:transparent;'>
<!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#2575FF;'><tr><td align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:650px'><tr class='layout-full-width' style='background-color:transparent'><![endif]-->
<!--[if (mso)|(IE)]><td align='center' width='650' style='background-color:transparent;width:650px; border-top: 0px solid transparent; border-left: 0px solid transparent; border-bottom: 0px solid transparent; border-right: 0px solid transparent;' valign='top'><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px; padding-left: 0px; padding-top:30px; padding-bottom:30px;'><![endif]-->
<div class='col num12' style='min-width: 320px; max-width: 650px; display: table-cell; vertical-align: top; width: 650px;'>
<div style='width:100% !important;'>
<!--[if (!mso)&(!IE)]><!-->
<div style='border-top:0px solid transparent; border-left:0px solid transparent; border-bottom:0px solid transparent; border-right:0px solid transparent; padding-top:30px; padding-bottom:30px; padding-right: 0px; padding-left: 0px;'>
<!--<![endif]-->
<!--[if mso]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 10px; padding-left: 10px; padding-top: 10px; padding-bottom: 10px; font-family: Tahoma, Verdana, sans-serif'><![endif]-->
<div style='color:#FFFFFF;font-family:Tahoma, Verdana, Segoe, sans-serif;line-height:1.2;padding-top:10px;padding-right:10px;padding-bottom:10px;padding-left:10px;'>
<div style='font-family: Tahoma, Verdana, Segoe, sans-serif; font-size: 12px; line-height: 1.2; color: #FFFFFF; mso-line-height-alt: 14px;'>
<p style='font-size: 14px; line-height: 1.2; word-break: break-word; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 17px; margin: 0;'>SMP 2019 © all rights reserved</p>
</div>
</div>
<!--[if mso]></td></tr></table><![endif]-->
<!--[if (!mso)&(!IE)]><!-->
</div>
<!--<![endif]-->
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
<!--[if (mso)|(IE)]></td></tr></table></td></tr></table><![endif]-->
</div>
</div>
</div>
<!--[if (mso)|(IE)]></td></tr></table><![endif]-->
</td>
</tr>
</tbody>
</table>
<!--[if (IE)]></div><![endif]-->
</body>
</html>";
    }
}