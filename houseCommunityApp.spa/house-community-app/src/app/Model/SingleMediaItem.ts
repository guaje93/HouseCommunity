import { SafeUrl } from '@angular/platform-browser';

export class SingleMediaItem {
    public ImageUrl: SafeUrl;
    public FileName: string;
    public Description: string;
    public CreationDate: Date;
    public AcceptanceDate: Date;
    public CurrentValue: number;
    public MediaType: string;
}

export class MediaToUpdate {
    public Id: number;
    public StartPeriodDate: Date;
    public EndPeriodDate: Date;
    public LastValue: number;
    public MediaType: string;
}
