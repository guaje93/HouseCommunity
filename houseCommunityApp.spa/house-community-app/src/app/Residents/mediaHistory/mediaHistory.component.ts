import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ImagePreviewComponent } from '../ImagePreview/ImagePreview.component';
import { SingleMediaItem } from '../../Model/SingleMediaItem';

@Component({
  selector: 'app-mediaHistory',
  templateUrl: './mediaHistory.component.html',
  styleUrls: ['./mediaHistory.component.scss']
})
export class MediaHistoryComponent implements OnInit, OnChanges {

  displayedColumns: string[] = ['MediaType', 'Description', 'CreationDate', 'AcceptanceDate', 'CurrentValue', 'ImageUrl'];
  @Input() 
  imageToShow: MatTableDataSource<SingleMediaItem>;
 
  
  constructor(    public dialog: MatDialog,
    ) {   
    }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes);
    // changes.prop contains the old and the new value...
  }

  @ViewChild(MatSort) sort: MatSort;

  ngAfterViewInit() {
    this.imageToShow.sort = this.sort;
  }

  openDialog(imageUrl: string): void {
    const dialogRef = this.dialog.open(ImagePreviewComponent, {
      width: '80%',
      data: imageUrl
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }

}
