import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatBottomSheetRef, MAT_BOTTOM_SHEET_DATA } from '@angular/material/bottom-sheet';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-UnlockPaymentForm',
  templateUrl: './UnlockPaymentForm.component.html',
  styleUrls: ['./UnlockPaymentForm.component.less']
})
export class UnlockPaymentFormComponent implements OnInit {

  public statusForm: FormControl = new FormControl('', [Validators.required]);
  constructor(private _bottomSheetRef: MatBottomSheetRef<UnlockPaymentFormComponent>,
    private authService: AuthService,
    @Inject(MAT_BOTTOM_SHEET_DATA) public data: any,
    private paymentService: PaymentService,
    private alertifyService: AlertifyService
    ) { }

  ngOnInit() {

  }

  openLink() {
    if(!this.statusForm.invalid){

      let model: any = {};
      model.paymentId = (this.data as any).id;
      model.userId = this.authService.decodedToken.nameid;
      model.status = +this.statusForm.value;
      console.log(model);
      this.paymentService.unlockPayment(model).subscribe(
        data => {
          this._bottomSheetRef.dismiss();
        }
        )
      }
      else{
        this.alertifyService.error('Wybierz status płatności')
      }
  }

}
