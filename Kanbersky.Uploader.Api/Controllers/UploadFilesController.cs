using Kanbersky.Uploader.Business.Abstract;
using Kanbersky.Uploader.Business.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbersky.Uploader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        #region fields

        private readonly IFileUploaderService _fileUploaderService;

        #endregion

        #region ctor

        public UploadFilesController(IFileUploaderService fileUploaderService)
        {
            _fileUploaderService = fileUploaderService;
        }

        #endregion

        #region methods

        /// <summary>
        /// Blob'da yüklü olan dosya ismi.extensions'a göre file'ı geri döndürür
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var response = await _fileUploaderService.GetByFileNameUploadAsync(fileName);
            return File(response.Content, response.ContentType);
        }

        /// <summary>
        /// Blob'da yüklü dosyaların ismini döner
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFile()
        {
            var response = await _fileUploaderService.GetAllUploadAsync();
            if (response != null && response.Any())
            {
                return Ok(response);
            }

            return NotFound();
        }

        /// <summary>
        /// Blob'a dosya yükleme işlemini yapar
        /// </summary>
        /// <param name="uploadFileRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-file")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequestModel uploadFileRequestModel)
        {
            await _fileUploaderService.UploadFileAsync(uploadFileRequestModel.FileName, uploadFileRequestModel.FilePath);
            return Ok();
        }

        /// <summary>
        /// Blob'a İçerik yükleme işlemini yapar
        /// </summary>
        /// <param name="uploadContentRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-content")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadContent([FromBody] UploadContentRequestModel uploadContentRequestModel)
        {
            await _fileUploaderService.UploadContentAsync(uploadContentRequestModel.FileName,uploadContentRequestModel.Content);
            return Ok();
        }

        /// <summary>
        /// Blob'daki file'ı silme işlemini yapar.
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{blobName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFile(string blobName)
        {
            await _fileUploaderService.DeleteFileAsync(blobName);
            return NoContent();
        }

        #endregion
    }
}