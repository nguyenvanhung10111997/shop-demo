import { Routes } from "@angular/router";

export const ROUTES: Routes = [
  {
    path: "",
    loadChildren: () => import("./products/product.routes").then(m => m.ROUTES),
  },
  {
    path: "orders",
    loadChildren: () => import("./orders/order.routes").then(m => m.ROUTES),
  },
  {
    path: "products",
    loadChildren: () => import("./products/product.routes").then(m => m.ROUTES),
  },
  {
    path: "cart",
    loadChildren: () => import("./cart/cart.routes").then(m => m.ROUTES),
  },
//   {
//     path: "page-500",
//     component: Page500Component
//   },
//   {
//     path: "page-503",
//     component: Page503Component
//   },
//   {
//     path: "page-404",
//     loadChildren: () =>
//       import("./error/page404/page404.config").then(m => m.ROUTES)
//   },
//   { path: "**", redirectTo: "/page-404", pathMatch: "full" }
];