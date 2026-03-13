import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProductoDTO, ProductoService } from '../../../services/producto.service';

@Component({
  selector: 'app-producto-detail',
  templateUrl: './producto-detail.component.html',
  styleUrl: './producto-detail.component.css'
})
export class ProductoDetailComponent implements OnInit {
  item?: ProductoDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private productoService: ProductoService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) this.loadItem(id);
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.productoService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || 'Error al cargar el producto';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

}
