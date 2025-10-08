import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "../../shared/components/header/header.component";

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './main-layout.component.html',
  standalone: true
})
export class MainLayoutComponent {

}
