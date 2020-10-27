/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { BlobService } from './blob.service';

describe('Service: Blob', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BlobService]
    });
  });

  it('should ...', inject([BlobService], (service: BlobService) => {
    expect(service).toBeTruthy();
  }));
});
