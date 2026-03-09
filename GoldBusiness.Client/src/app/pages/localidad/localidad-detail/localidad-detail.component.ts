import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { LocalidadService, LocalidadDTO } from '../../../services/localidad.service';

@Component({
  selector: 'app-localidad-detail',
  templateUrl: './localidad-detail.component.html',
  styleUrls: ['./localidad-detail.component.css']
})
export class LocalidadDetailComponent implements OnInit {
  item?: LocalidadDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private localidadService: LocalidadService,
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

    this.localidadService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading localidad:', error);
        this.error = error.message || 'Error al cargar la localidad';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
