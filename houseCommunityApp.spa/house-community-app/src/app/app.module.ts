import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './residents/nav/nav.component';
import { LogInComponent } from './logIn/logIn.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutes } from './routes';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './residents/home/home.component';
import { MediaComponent } from './residents/Media/Media.component';
import { AddMediaFormComponent } from './residents/AddMediaForm/AddMediaForm.component';
import { NewsComponent } from './residents/News/News.component';
import { RentComponent } from './residents/rent/rent.component';
import { EditUserComponent } from './editUser/editUser.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldControl, MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ForgetPasswordComponent } from './forgetPassword/forgetPassword.component';
import { ResetPasswordComponent } from './resetPassword/resetPassword.component';
import { ChangePasswordComponent } from './changePassword/changePassword.component';
import { DragAndDropDirective } from './directives/dragAndDrop.directive';
import { CommonModule, DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ImagePreviewComponent } from './residents/ImagePreview/ImagePreview.component';
import { MatSortModule } from '@angular/material/sort';
import { MediaHistoryComponent } from './residents/mediaHistory/MediaHistory.component';
import { PaymentDetailsComponent } from './residents/paymentDetails/paymentDetails.component';
import { ChatComponent } from './chat/chat.component';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { PaymentsAdministrationComponent } from './administration/paymentsAdministration/paymentsAdministration.component';
import { MediaAdministrationComponent } from './administration/mediaAdministration/mediaAdministration.component';
import { AnnouncementsAdministrationComponent } from './administration/announcementsAdministration/announcementsAdministration.component';
import { UserRoleDirective } from './directives/userRole.directive';
import { UserDirective } from './directives/user.directive';
import { PaymentFormComponent } from './administration/paymentForm/paymentForm.component';
import { DamageComponent } from './residents/Damage/Damage.component';
import { RegisterUserComponent } from './buildingOwners/RegisterUser/RegisterUser.component';
import {MatTooltipModule} from '@angular/material/tooltip';
import { RegisterFormComponent } from './buildingOwners/registerForm/registerForm.component';
import {MatCardModule} from '@angular/material/card'
import { DamageManageComponent } from './BuildingOwners/damageManage/damageManage.component';
import { DamageInfoComponent } from './BuildingOwners/DamageInfo/DamageInfo.component';
import {MatRadioModule} from '@angular/material/radio'
import {MatCheckboxModule} from '@angular/material/checkbox'

@NgModule({
  declarations: [							
    AppComponent,
    NavComponent,
    LogInComponent,
    HomeComponent,
    MediaComponent,
    NewsComponent,
    RentComponent,
    EditUserComponent,
    ForgetPasswordComponent,
    ResetPasswordComponent,
    ChangePasswordComponent,
    DragAndDropDirective,
      ImagePreviewComponent,
      AddMediaFormComponent,
      MediaHistoryComponent,
      PaymentDetailsComponent,
      ChatComponent,
      PaymentsAdministrationComponent,
      MediaAdministrationComponent,
      AnnouncementsAdministrationComponent,
      PaymentFormComponent,
      DamageComponent,
      UserDirective,
    UserRoleDirective,
    RegisterUserComponent,
    RegisterFormComponent,
    DamageManageComponent,
    DamageInfoComponent
   ],
  imports: [
    RouterModule.forRoot(AppRoutes),
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BsDropdownModule.forRoot(),
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
    PdfJsViewerModule,
    MatTooltipModule,
    CommonModule,
    MatCardModule,
    MatRadioModule,
    MatCheckboxModule
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
