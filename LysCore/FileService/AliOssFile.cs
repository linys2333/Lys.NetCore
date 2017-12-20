using System;
using System.ComponentModel.DataAnnotations;

namespace LysCore.FileService
{
    public class AliOssFile
    {
        [Required, RegularExpression(@"[a-zA-Z|\d]{1,50}", ErrorMessage = "app名称不能超过50个字符且只能为字母和数字！")]
        public string AppName { get; set; }

        [Required, StringLength(128)]
        public string Key { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public string OwnerId { get; set; }

        [Required, StringLength(128, MinimumLength = 1)]
        public string FileName { get; set; }

        public int CodePage { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string ETag { get; set; }

        public bool IsEncrypted { get; set; }

        [Required]
        public FileStatus FileStatus { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        public AliOssFile()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
