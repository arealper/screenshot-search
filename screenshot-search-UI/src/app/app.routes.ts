import { Routes } from '@angular/router';
import { SearchComponent } from './components/search/search.component';
import { UploadComponent } from './components/upload/upload.component';

export const routes: Routes = [
    { path: 'upload', component: UploadComponent },
    { path: 'search', component: SearchComponent },
    { path: '', redirectTo: '/upload', pathMatch: 'full' }
];
