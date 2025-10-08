import { Routes } from "@angular/router";
import { ProductComponent } from "./product.component";
import { MainLayoutComponent } from "../../layouts/main/main-layout.component";

export const ROUTES: Routes = [
  {
    path: "",
    component: MainLayoutComponent,
    children: [
      {
        path: "",
        component: ProductComponent
      }
    ]
  }
];