import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PurchaseBillService } from '../../core/services/purchase-bill';
import { PurchaseBillItem } from '../../core/models/purchase-bill.models';

@Component({
  selector: 'app-purchase-bill',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css',
})
export class PurchaseBill implements OnInit {
  itemForm: FormGroup;
  locations: string[] = [];
  itemOptions: string[] = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];

  submitMessage: string | null = null;
  submitSuccess = false;
  isSubmitting = false;

  addedItems: PurchaseBillItem[] = [];

  isLoadingLocations = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private purchaseBillService: PurchaseBillService,
  ) {
    this.itemForm = this.fb.group({
      item: ['', Validators.required],
      batch: ['', Validators.required],
      standardCost: [0, [Validators.required, Validators.min(0.01)]],
      standardPrice: [0, [Validators.required, Validators.min(0.01)]],
      quantity: [0, [Validators.required, Validators.min(1)]],
      freeQty: [0, [Validators.min(0)]],
      discount: [0, [Validators.min(0), Validators.max(100)]],
    });
  }

  ngOnInit(): void {
    this.isLoadingLocations = true;
    this.purchaseBillService.getLocations().subscribe({
      next: (locations) => {
        this.locations = locations;
        this.isLoadingLocations = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load locations.';
        this.isLoadingLocations = false;
        console.error(err);
      },
    });
  }

  get totalCost(): number {
    const cost = this.itemForm.value.standardCost || 0;
    const qty = this.itemForm.value.quantity || 0;
    const discount = this.itemForm.value.discount || 0;
    return cost * qty * (1 - discount / 100);
  }

  get totalSelling(): number {
    const price = this.itemForm.value.standardPrice || 0;
    const qty = this.itemForm.value.quantity || 0;
    return price * qty;
  }

  onAddItem(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    const formValue = this.itemForm.value;

    const newItem: PurchaseBillItem = {
      item: formValue.item,
      batch: formValue.batch,
      standardCost: formValue.standardCost,
      standardPrice: formValue.standardPrice,
      quantity: formValue.quantity,
      freeQty: formValue.freeQty,
      discount: formValue.discount,
      totalCost: this.totalCost,
      totalSelling: this.totalSelling,
    };

    this.addedItems.push(newItem);

    this.itemForm.reset({
      item: '',
      batch: '',
      standardCost: 0,
      standardPrice: 0,
      quantity: 0,
      freeQty: 0,
      discount: 0,
    });
  }

  get totalItemsCount(): number {
    return this.addedItems.length;
  }

  get totalQuantitySum(): number {
    return this.addedItems.reduce((sum, item) => sum + item.quantity, 0);
  }

  removeItem(index: number): void {
    this.addedItems.splice(index, 1);
  }

  onSubmitBill(): void {
    if (this.addedItems.length === 0) {
      this.submitMessage = 'Add at least one item before submitting.';
      this.submitSuccess = false;
      return;
    }

    this.isSubmitting = true;
    this.submitMessage = null;

    this.purchaseBillService.submit({ items: this.addedItems }).subscribe({
      next: (result) => {
        this.isSubmitting = false;
        this.submitSuccess = result.success;
        this.submitMessage = result.message;

        if (result.success) {
          this.addedItems = [];
        }
      },
      error: (err) => {
        this.isSubmitting = false;
        this.submitSuccess = false;
        this.submitMessage = 'Failed to submit purchase bill. Please try again.';
        console.error(err);
      },
    });
  }
}
