using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Global.Config
{
    public interface IConfig
    {
        string Lang { get; }

        bool EnableMail { get; }

        IQueryable<IconSize> IconSizes { get; }

        IQueryable<MimeType> MimeTypes { get; }

        IQueryable<MailTemplate> MailTemplates { get; }

        MailSetting MailSetting { get; }

    }
}