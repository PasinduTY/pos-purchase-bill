import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  PurchaseBillSubmitRequest,
  PurchaseBillSubmitResult,
} from '../models/purchase-bill.models';

@Injectable({
  providedIn: 'root',
})
export class PurchaseBillService {
  private readonly apiUrl = 'https://localhost:7021/api/PurchaseBill';

  constructor(private http: HttpClient) {}

  getLocations(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/locations`);
  }

  submit(request: PurchaseBillSubmitRequest): Observable<PurchaseBillSubmitResult> {
    return this.http.post<PurchaseBillSubmitResult>(`${this.apiUrl}/submit`, request);
  }
}
