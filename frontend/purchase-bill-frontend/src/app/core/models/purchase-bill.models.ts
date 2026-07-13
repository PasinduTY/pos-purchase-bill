export interface PurchaseBillItem {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

export interface PurchaseBillSubmitRequest {
  items: PurchaseBillItem[];
}

export interface PurchaseBillSubmitResult {
  success: boolean;
  message: string | null;
  totalItems: number;
  totalQuantity: number;
}
