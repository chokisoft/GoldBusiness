import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Establecimiento, EstablecimientoService } from '../../../services/establecimiento.service';

@Component({
  selector: 'app-establecimiento-detail',
  templateUrl: './establecimiento-detail.component.html',
  styleUrls: ['./establecimiento-detail.component.css']
})
export class EstablecimientoDetailComponent implements OnInit {
  item?: Establecimiento;
  loading = false;
  error: string | null = null;

  constructor(
    private establecimientoService: EstablecimientoService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadItem(id);
      }
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.establecimientoService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading establecimiento:', error);
        this.error = error.message || 'Error al cargar el establecimiento';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
