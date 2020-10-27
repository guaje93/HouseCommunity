import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';
import { DomSanitizer } from '@angular/platform-browser';
import { SingleMediaItem } from '../Model/SingleMediaItem';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { AnonymousCredential, BlobServiceClient, BlobUploadCommonResponse, newPipeline } from '@azure/storage-blob';

@Injectable({
  providedIn: 'root'
})
export class BlobService {

  containerName = environment.containerName;
  accountName = environment.accountName;
  key = environment.key;

  constructor(private sanitizer: DomSanitizer,
              private datePipe: DatePipe,
    ) { }


    async uploadFile(currentFile: File): Promise<BlobUploadCommonResponse>{

      const end = new Date(new Date().getTime() + (30 * 60 * 1000));
      const signedpermissions = 'rwdlac';
      const signedservice = 'b';
      const signedresourcetype = 'sco';
      const signedexpiry = end.toISOString().substring(0, end.toISOString().lastIndexOf('.')) + 'Z';
      const signedProtocol = 'https';
      const signedversion = '2018-03-28';

      const StringToSign =
        this.accountName + '\n' +
        signedpermissions + '\n' +
        signedservice + '\n' +
        signedresourcetype + '\n' +
        '\n' +
        signedexpiry + '\n' +
        '\n' +
        signedProtocol + '\n' +
        signedversion + '\n';

      const str = CryptoJS.HmacSHA256(StringToSign, CryptoJS.enc.Base64.parse(this.key));
      const sig = CryptoJS.enc.Base64.stringify(str);


      const sasToken = `sv=${(signedversion)}&ss=${(signedservice)}&srt=${(signedresourcetype)}&sp=${(signedpermissions)}&se=${encodeURIComponent(signedexpiry)}&spr=${(signedProtocol)}&sig=${encodeURIComponent(sig)}`;
      const containerName = environment.containerName;

      const pipeline = newPipeline(new AnonymousCredential(), {
        retryOptions: { maxTries: 4 }, // Retry options
        userAgentOptions: { userAgentPrefix: 'AdvancedSample V1.0.0' }, // Customized telemetry string
        keepAliveOptions: {
          // Keep alive is enabled by default, disable keep alive by setting false
          enable: false
        }
      });

      const blobServiceClient = new BlobServiceClient(`https://${this.accountName}.blob.core.windows.net?${sasToken}`,
        pipeline);
      const containerClient = blobServiceClient.getContainerClient(containerName);
      if (!containerClient.exists()) {
        console.log('the container does not exit');
        await containerClient.create();

      }
      const client = containerClient.getBlockBlobClient(currentFile.name);
      const response = await client.uploadBrowserData(currentFile, {
        blockSize: 4 * 1024 * 1024, // 4MB block size
        concurrency: 20, // 20 concurrency
        onProgress: (ev) => console.log(ev),
        blobHTTPHeaders: { blobContentType: currentFile.type }
      });
      return response;
    }

    createRequestForAddingFile(file: any, fileName: string, id: any){
      const req: any = {};
      req.UserId = id;
      req.ImageUrl = `https://${this.accountName}.blob.core.windows.net/${this.containerName}/${fileName}`;
      req.UserDescription = file.description;
      req.MediaType = file.type;
      console.log(req);
      return req;
    }

    formatBytes(bytes, decimals = 2): string {
      if (bytes === 0) {
        return '0 Bytes';
      }
      const k = 1024;
      const dm = decimals <= 0 ? 0 : decimals;
      const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
      const i = Math.floor(Math.log(bytes) / Math.log(k));
      return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
    }
}
