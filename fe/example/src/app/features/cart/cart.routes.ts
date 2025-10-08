import { Routes } from "@angular/router";
import { MainLayoutComponent } from "../../layouts/main/main-layout.component";
import { CartComponent } from "./cart.component";

export const ROUTES: Routes = [
  {
    path: "",
    component: MainLayoutComponent,
    children: [
      {
        path: "",
        component: CartComponent
      }
    ]
  }
];