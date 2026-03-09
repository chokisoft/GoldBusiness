import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';

@Component({
  selector: 'app-municipio-detail',
  templateUrl: './municipio-detail.component.html',
  styleUrls: ['./municipio-detail.component.css']
})
export class MunicipioDetailComponent implements OnInit {
  item?: MunicipioDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private municipioService: MunicipioService,
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

    this.municipioService.getById(id).subscribe({
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading municipio:', error);
        this.error = error.message || 'Error al cargar el municipio';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
