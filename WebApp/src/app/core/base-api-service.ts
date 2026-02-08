import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.dev';
import { inject } from '@angular/core';

export abstract class BaseApiService<T> {
  protected apiUrl = `${environment.BASE_URL}/api`;
  protected abstract endpoint: string;
  protected http = inject(HttpClient);

  getAll(params?: any): Observable<T[]> {
    const httpParams = this.buildParams(params);
    return this.http.get<T[]>(`${this.apiUrl}/${this.endpoint}`, { params: httpParams });
  }

  getById(id: number | string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${this.endpoint}/${id}`);
  }

  create(item: Partial<T>): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${this.endpoint}`, item, {
      headers: this.getHeaders()
    });
  }

  update(id: number | string, item: Partial<T>): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${this.endpoint}/${id}`, item, {
      headers: this.getHeaders()
    });
  }

  patch(id: number | string, updates: Partial<T>): Observable<T> {
    return this.http.patch<T>(`${this.apiUrl}/${this.endpoint}/${id}`, updates, {
      headers: this.getHeaders()
    });
  }

  delete(id: number | string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${this.endpoint}/${id}`);
  }

  protected get<R>(url: string, params?: any): Observable<R> {
    const httpParams = this.buildParams(params);
    return this.http.get<R>(`${this.apiUrl}/${url}`, { params: httpParams });
  }

  protected post<R>(url: string, data: any): Observable<R> {
    return this.http.post<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    });
  }

  protected put<R>(url: string, data: any): Observable<R> {
    return this.http.put<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    });
  }

  protected patchRequest<R>(url: string, data: any): Observable<R> {
    return this.http.patch<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    });
  }

  protected deleteRequest<R>(url: string): Observable<R> {
    return this.http.delete<R>(`${this.apiUrl}/${url}`);
  }

  protected getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  protected buildParams(params?: any): HttpParams {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }
    return httpParams;
  }
}