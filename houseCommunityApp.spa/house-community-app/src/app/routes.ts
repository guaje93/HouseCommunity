import { Routes } from '@angular/router';
import { ChatComponent } from './chat/chat.component';
import { ForgetPasswordComponent } from './forgetPassword/forgetPassword.component';
import { HomeComponent } from './residents/home/home.component';
import { LogInComponent } from './logIn/logIn.component';
import { MediaComponent } from './residents/Media/Media.component';
import { NewsComponent } from './residents/News/News.component';
import { RentComponent } from './residents/rent/rent.component';
import { ResetPasswordComponent } from './resetPassword/resetPassword.component';
import { AuthGuard } from './_guards/auth.guard';
import { PaymentsAdministrationComponent } from './administration/paymentsAdministration/paymentsAdministration.component';
import { AnnouncementsAdministrationComponent } from './administration/announcementsAdministration/announcementsAdministration.component';
import { MediaAdministrationComponent } from './administration/mediaAdministration/mediaAdministration.component';
import { DamageComponent } from './residents/Damage/Damage.component';
import { RegisterUserComponent } from './buildingOwners/RegisterUser/RegisterUser.component';
import { DamageManageComponent } from './BuildingOwners/damageManage/damageManage.component';

export const AppRoutes: Routes = [


    { path: 'login', component: LogInComponent },
    { path: 'forgotPassword', component: ForgetPasswordComponent },
    { path: 'response-reset-password/:token', component: ResetPasswordComponent },
    { path: '', component: LogInComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'home', component: HomeComponent },
            { path: 'paymentsAdministration', component: PaymentsAdministrationComponent },
            { path: 'announcementsAdministration', component: AnnouncementsAdministrationComponent },
            { path: 'mediaAdministration', component: MediaAdministrationComponent },
            { path: 'media', component: MediaComponent },
            { path: 'news', component: NewsComponent },
            { path: 'rent', component: RentComponent },
            { path: 'chat', component: ChatComponent },
            { path: 'damage', component: DamageComponent },
            { path: 'damageManage', component: DamageManageComponent },
            { path: 'register', component: RegisterUserComponent },
        ]
    },
    { path: '**', redirectTo: '/login', pathMatch: 'full' },
];
