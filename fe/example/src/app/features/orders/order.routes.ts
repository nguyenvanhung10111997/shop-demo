import { Routes } from "@angular/router";
import { MainLayoutComponent } from "../../layouts/main/main-layout.component";
import { OrderComponent } from "./order.component";

export const ROUTES: Routes = [
  {
    path: "",
    component: MainLayoutComponent,
    children: [
      {
        path: "",
        component: OrderComponent
      }
    ]
  }
];