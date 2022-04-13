
using Microsoft.AspNetCore.Mvc;
namespace Encrption.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecurityController : ControllerBase
{
    [HttpPost("Encrypt")]
    public ActionResult<IEnumerable<string>> Encrypt([FromBody] Encrption decrypted)
    {
        var serviceCollection = new ServiceCollection();

        //add protection service
        serviceCollection.AddDataProtection();
        var lockerKey = serviceCollection.BuildServiceProvider();


        //create an instance of classfile using 'CreateInstance' method
        var locker = ActivatorUtilities.CreateInstance<Security>(lockerKey);
        string encryptKey = locker.Encrypt(decrypted.Text);
        string deencryptKey = locker.Decrypt(encryptKey);

        return new string[] { encryptKey, deencryptKey };
    }

    [HttpPost("Decrypt")]
    public ActionResult<IEnumerable<string>> Decrypt([FromBody] Encrption encrpted)
    {
        var serviceCollection = new ServiceCollection();

        //add protection service

        serviceCollection.AddDataProtection();
        var lockerKey = serviceCollection.BuildServiceProvider();

        //create an instance of classfile using 'CreateInstance' method
        var locker = ActivatorUtilities.CreateInstance<Security>(lockerKey);

        string deencryptKey = locker.Decrypt(encrpted.Text);

        return new string[] { deencryptKey };
    }
}