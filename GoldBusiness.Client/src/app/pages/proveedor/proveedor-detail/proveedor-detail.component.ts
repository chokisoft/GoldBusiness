import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProveedorDTO, ProveedorService } from '../../../services/proveedor.service';

@Component({
  selector: 'app-proveedor-detail',
  templateUrl: './proveedor-detail.component.html',
  styleUrl: './proveedor-detail.component.css'
})
export class ProveedorDetailComponent implements OnInit {
  item?: ProveedorDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private proveedorService: ProveedorService,
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

    this.proveedorService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || 'Error al cargar el proveedor';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

}
