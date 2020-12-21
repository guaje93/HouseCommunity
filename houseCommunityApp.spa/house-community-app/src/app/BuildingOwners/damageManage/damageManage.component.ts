import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { DamageService } from 'src/app/_services/damage.service';
import { DamageInfoComponent } from '../DamageInfo/DamageInfo.component';

@Component({
  selector: 'app-damageManage',
  templateUrl: './damageManage.component.html',
  styleUrls: ['./damageManage.component.scss']
})
export class DamageManageComponent implements OnInit {

  damages = [];
  fixedDamages = [];
  dataSource = new MatTableDataSource(this.damages);
  historyDataSource = new MatTableDataSource(this.damages);

  displayedColumns: string[] = ['Name', 'CreationDate', 'Author', 'Preview', 'Status'];

  constructor(private authService: AuthService, private alertifyService: AlertifyService, private damageService: DamageService, private dialog: MatDialog) {


  }

  ngOnInit() {
    this.LoadData();
  }

  private LoadData() {
    this.damageService.getDamagesForHouseManager(this.authService.decodedToken.nameid).subscribe(data => {
      console.log(data);
      this.damages = (data as any[]);

      this.dataSource.data = this.damages;
      console.log(this.dataSource);
    });

    this.damageService.getFixedDamagesForHouseManager(this.authService.decodedToken.nameid).subscribe(data => {
      console.log(data);
      this.fixedDamages = (data as any[]);

      this.historyDataSource.data = this.fixedDamages;
      console.log(this.historyDataSource);
    });
  }

  openDialog(element: any): void {
    const dialogRef = this.dialog.open(DamageInfoComponent, {
      width: '80%',
      data: element
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }

  markDamageAsFixed(element: any) {
    this.damageService.markDamageAsFixed(element.id).subscribe(
      data => {
        console.log(data);
        this.alertifyService.success("Zapisano!");
        this.LoadData();

      }

    )
  }
  markDamageAsNotFixed(element: any) {
    this.damageService.markDamageAsNotFixed(element.id).subscribe(
      data => {
        console.log(data);
        this.alertifyService.success("Zapisano!");
        this.LoadData();

      }


    )
  }
}