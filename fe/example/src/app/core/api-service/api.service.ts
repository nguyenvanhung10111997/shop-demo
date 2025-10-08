import { HttpClient, HttpEventType, HttpHeaders, HttpParams, HttpRequest, HttpResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, filter, map, Observable, tap, throwError } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class ApiService {
  private _http = inject(HttpClient);

  constructor() {
  }

  get<T>(url: string, params?: HttpParams): Observable<T> {
    return this.sendRequest<T>("get", url, null, params);
  }

  delete<T>(url: string, id: string): Observable<T> {
    return this.sendRequest<T>("delete", `${url}/${id}`);
  }

  put<T>(url: string, value: any): Observable<T> {
    return this.sendRequest<T>("put", url, value);
  }

  post<T>(url: string, value?: any): Observable<T> {
    return this.sendRequest<T>("post", url, value);
  }

  private sendRequest<T>(
    method: string,
    url: string,
    data: any | null = null,
    params?: HttpParams
  ): Observable<T> {
    const requestOptions: HttpRequest<any> = new HttpRequest(
      method,
      url,
      data,
      {
        headers: this.setHeaders(),
        params: params,
        //withCredentials: true,
        responseType: "json"
      }
    );

    return this._http.request<any>(requestOptions).pipe(
      filter(event => event && event.type === HttpEventType.Response),
      map((response: HttpResponse<any>) => response.body),
      map(apiResponse => apiResponse as T),
      catchError(error => {
        if (error && error.status === 400) {
          return throwError({
            status: error.status,
            message: "Invalid request. Please check your input."
          });
        }

        console.error('Error loading order statuses:', error);
        return throwError(error);
      })
    );
  }

  private setHeaders = () => {
    let headers = new HttpHeaders();
    headers = headers.set(
      "ContextID",
      this.generateUUID().toString().replace(/-/g, "")
    );
    headers = headers.set("Accept", "application/json");
    headers = headers.set("Content-Type", "application/json; charset=utf-8");

    return headers;
  };

  public objectToQueryString(obj: { [key: string]: any }): string {
    const queryParams = [];

    for (const key in obj) {
      if (obj.hasOwnProperty(key) && obj[key] !== undefined) {
        if (obj[key] != null) {
          const value = encodeURIComponent(obj[key]);
          queryParams.push(`${encodeURIComponent(key)}=${value}`);
        }
      }
    }

    return queryParams.join("&");
  }

  generateUUID() {
    let dt = new Date().getTime();
    const uuid = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        const r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c === "x" ? r : (r & 0x3) | 0x8).toString(16);
      }
    );
    return uuid.replace(/-/g, "");
  }
}