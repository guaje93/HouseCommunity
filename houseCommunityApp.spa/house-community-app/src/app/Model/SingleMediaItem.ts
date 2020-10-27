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
