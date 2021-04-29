import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdventOfCodeComponent } from './advent-of-code/advent-of-code.component';
import { ContactBookComponent } from './contact-book/contact-book.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: 'advent-of-code', component: AdventOfCodeComponent },
  { path: 'contact-book', component: ContactBookComponent },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
