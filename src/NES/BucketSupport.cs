using System;

namespace NES
{
    public class BucketSupport
    {
        private static string defaultBucketId = "default";

        public static string DefaultBucketId
        {
            get { return defaultBucketId; }

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(DefaultBucketId);
                }

                defaultBucketId = value;
            }
        }
    }
}