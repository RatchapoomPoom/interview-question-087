import { Component, OnInit, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import JsBarcode from 'jsbarcode';
import { Product, ProductService } from '../../services/product.service';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ConfirmDialog],
  templateUrl: './product-list.html',
  styleUrls: ['./product-list.scss']
})
export class ProductList implements OnInit, AfterViewChecked {
  products: Product[] = [];
  newProductCode = '';
  errorMessage = '';

  showConfirm = false;
  productToDelete: Product | null = null;

  private barcodesRendered = false;

constructor(
    private productService: ProductService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  ngAfterViewChecked(): void {
    if (!this.barcodesRendered) {
      this.renderBarcodes();
      this.barcodesRendered = true;
    }
  }

loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
        this.barcodesRendered = false;
        this.cdr.detectChanges(); // บังคับ Angular render หน้าจอใหม่ทันที
      },
      error: () => {
        this.errorMessage = 'ไม่สามารถโหลดข้อมูลได้';
        this.cdr.detectChanges();
      }
    });
  }

  renderBarcodes(): void {
    setTimeout(() => {
      this.products.forEach((p) => {
        const el = document.getElementById('barcode-' + p.id);
        if (el) {
          try {
            JsBarcode(el, p.productCode, {
              format: 'CODE39',
              displayValue: false,
              height: 40,
              width: 1.5,
              margin: 0
            });
          } catch (e) {
            // ถ้ารูปแบบไม่ถูกต้องสำหรับ Code39 จะข้ามการวาด
          }
        }
      });
    });
  }
    onCodeInput(value: string): void {
        // ตัดอักขระที่ไม่ใช่ตัวเลข/อังกฤษออก แล้วแปลงเป็นตัวพิมพ์ใหญ่
        const raw = value.replace(/[^0-9A-Za-z]/g, '').toUpperCase().slice(0, 16);

        // ใส่ขีดคั่นทุก 4 ตัว
        const groups = raw.match(/.{1,4}/g) || [];
        this.newProductCode = groups.join('-');
    }
onAdd(): void {
    this.errorMessage = '';
    const code = this.newProductCode.trim().toUpperCase();

    const pattern = /^[0-9A-Z]{4}-[0-9A-Z]{4}-[0-9A-Z]{4}-[0-9A-Z]{4}$/;
    if (!pattern.test(code)) {
      this.errorMessage = 'รูปแบบรหัสสินค้าไม่ถูกต้อง (ต้องเป็น xxxx-xxxx-xxxx-xxxx ตัวเลข/อังกฤษพิมพ์ใหญ่)';
      this.cdr.detectChanges();
      Swal.fire({
        icon: 'error',
        title: 'รูปแบบไม่ถูกต้อง',
        text: this.errorMessage,
        confirmButtonText: 'ตกลง'
      });
      return;
    }

    Swal.fire({
      icon: 'question',
      title: 'ยืนยันการเพิ่มข้อมูล',
      text: 'ต้องการเพิ่มรหัสสินค้า ' + code + ' หรือไม่ ?',
      showCancelButton: true,
      confirmButtonText: 'ตกลง',
      cancelButtonText: 'ยกเลิก'
    }).then((result) => {
      if (!result.isConfirmed) {
        return;
      }

      this.productService.create(code).subscribe({
        next: () => {
          this.newProductCode = '';
          this.loadProducts();
          Swal.fire({
            icon: 'success',
            title: 'เพิ่มข้อมูลสำเร็จ',
            text: 'รหัสสินค้า ' + code + ' ถูกเพิ่มเรียบร้อยแล้ว',
            timer: 1500,
            showConfirmButton: false
          });
        },
        error: (err: any) => {
          this.errorMessage = (err && err.error && err.error.message) ? err.error.message : 'เกิดข้อผิดพลาดในการเพิ่มข้อมูล';
          this.cdr.detectChanges();
          Swal.fire({
            icon: 'error',
            title: 'เพิ่มข้อมูลไม่สำเร็จ',
            text: this.errorMessage,
            confirmButtonText: 'ตกลง'
          });
        }
      });
    });
  }

  onDeleteClick(product: Product): void {
    this.productToDelete = product;
    this.showConfirm = true;
  }

  onConfirmDelete(): void {
    if (!this.productToDelete) {
      return;
    }
    const deletedCode = this.productToDelete.productCode;
    this.productService.delete(this.productToDelete.id).subscribe({
      next: () => {
        this.showConfirm = false;
        this.productToDelete = null;
        this.loadProducts();
        Swal.fire({
          icon: 'success',
          title: 'ลบข้อมูลสำเร็จ',
          text: 'รหัสสินค้า ' + deletedCode + ' ถูกลบเรียบร้อยแล้ว',
          timer: 1500,
          showConfirmButton: false
        });
      },
      error: () => {
        this.errorMessage = 'ไม่สามารถลบข้อมูลได้';
        this.showConfirm = false;
        Swal.fire({
          icon: 'error',
          title: 'ลบข้อมูลไม่สำเร็จ',
          text: this.errorMessage,
          confirmButtonText: 'ตกลง'
        });
      }
    });
  }

  onCancelDelete(): void {
    this.showConfirm = false;
    this.productToDelete = null;
  }
}