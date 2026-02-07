import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { environment } from '../../environment/environment.dev';
import { inject } from '@angular/core';

export abstract class BaseApiService<T> {
  protected apiUrl = `${environment.BASE_URL}/api`;
  protected abstract endpoint: string;
  protected http = inject(HttpClient);

  getAll(params?: any): Observable<T[]> {
    const httpParams = this.buildParams(params);
    return this.http.get<T[]>(`${this.apiUrl}/${this.endpoint}`, { params: httpParams })
      .pipe(
        catchError(this.handleError)
      );
  }

  getById(id: number | string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${this.endpoint}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  create(item: Partial<T>): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${this.endpoint}`, item, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  update(id: number | string, item: Partial<T>): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${this.endpoint}/${id}`, item, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  patch(id: number | string, updates: Partial<T>): Observable<T> {
    return this.http.patch<T>(`${this.apiUrl}/${this.endpoint}/${id}`, updates, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(id: number | string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${this.endpoint}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  protected get<R>(url: string, params?: any): Observable<R> {
    const httpParams = this.buildParams(params);
    return this.http.get<R>(`${this.apiUrl}/${url}`, { params: httpParams })
      .pipe(
        catchError(this.handleError)
      );
  }

  protected post<R>(url: string, data: any): Observable<R> {
    return this.http.post<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  protected put<R>(url: string, data: any): Observable<R> {
    return this.http.put<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  protected patchRequest<R>(url: string, data: any): Observable<R> {
    return this.http.patch<R>(`${this.apiUrl}/${url}`, data, {
      headers: this.getHeaders()
    })
      .pipe(
        catchError(this.handleError)
      );
  }

  protected deleteRequest<R>(url: string): Observable<R> {
    return this.http.delete<R>(`${this.apiUrl}/${url}`)
      .pipe(
        catchError(this.handleError)
      );
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

  protected handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    console.error(errorMessage);
    return throwError(() => error);
  }
}