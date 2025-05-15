using CloudinaryDotNet.Actions;
using dotenv.net;
using VacApp_Bovinova_Platform.Shared.Application.OutboundServices;
using CloudinarySdk = CloudinaryDotNet;


namespace VacApp_Bovinova_Platform.Shared.Infrastructure.Media.Cloudinary
{
    public class CloudinaryService : IMediaStorageService
    {
        private readonly CloudinarySdk.Cloudinary cloudinary;

        public CloudinaryService()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            cloudinary = new CloudinarySdk.Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;
        }

        public string UploadFileAsync(string fileName, Stream fileData)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new CloudinarySdk.FileDescription(fileName, fileData),
                Format = "webp"
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }
    }
}