import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ClienteDTO, ClienteService } from '../../../services/cliente.service';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { MunicipioService, MunicipioDTO } from '../../../services/municipio.service';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';

@Component({
  selector: 'app-cliente-form',
  templateUrl: './cliente-form.component.html',
  styleUrls: ['./cliente-form.component.css']
})
export class ClienteFormComponent implements OnInit, OnDestroy {
  itemForm: FormGroup;
  isEditMode = false;
  itemId?: number;
  loading = false;
  saving = false;
  error: string | null = null;

  paises: PaisDTO[] = [];
  provincias: ProvinciaDTO[] = [];
  municipios: MunicipioDTO[] = [];
  codigosPostales: CodigoPostalDTO[] = [];

  private subs: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private paisService: PaisService,
    private provinciaService: ProvinciaService,
    private municipioService: MunicipioService,
    private codigoPostalService: CodigoPostalService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.itemForm = this.fb.group({
      codigo: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8)]],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      nif: ['', Validators.maxLength(11)],
      iban: ['', Validators.maxLength(27)],
      bicoSwift: ['', Validators.maxLength(11)],
      iva: [0, [Validators.required]],
      direccion: ['', Validators.maxLength(256)],
      paisId: [null],
      provinciaId: [null],
      municipioId: [null],
      codigoPostalId: [null],
      web: ['', Validators.maxLength(256)],
      email1: ['', [Validators.email, Validators.maxLength(256)]],
      email2: ['', [Validators.email, Validators.maxLength(256)]],
      telefono1: ['', Validators.maxLength(50)],
      telefono2: ['', Validators.maxLength(50)],
      fax1: ['', Validators.maxLength(50)],
      fax2: ['', Validators.maxLength(50)],
      cancelado: [false]
    });

    // Inicialmente deshabilitar controles dependientes
    this.itemForm.get('provinciaId')!.disable();
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();
  }

  ngOnInit(): void {
    this.loadPaises();

    this.subs.push(
      this.itemForm.get('paisId')!.valueChanges.subscribe(v => this.onPaisChange(v))
    );
    this.subs.push(
      this.itemForm.get('provinciaId')!.valueChanges.subscribe(v => this.onProvinciaChange(v))
    );
    this.subs.push(
      this.itemForm.get('municipioId')!.valueChanges.subscribe(v => this.onMunicipioChange(v))
    );

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = +params['id'];
        this.isEditMode = true;
        this.loadItem();
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  private loadPaises(): void {
    this.paisService.getAll().subscribe({
      next: list => this.paises = list,
      error: err => console.error('Error loading paises', err)
    });
  }

  private loadProvincias(paisId?: number): void {
    this.provincias = [];
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
    // deshabilitar hijos hasta que se habilite el control correspondiente
    this.itemForm.get('municipioId')!.disable();
    this.itemForm.get('codigoPostalId')!.disable();

    if (!paisId) {
      this.itemForm.get('provinciaId')!.disable();
      return;
    }

    // habilitar control provincia antes de poblar la lista
    this.itemForm.get('provinciaId')!.enable();
    this.provinciaService.getByPaisId(paisId).subscribe({
      next: list => this.provincias = list,
      error: err => console.error('Error loading provincias', err)
    });
  }

  private loadMunicipios(provinciaId?: number): void {
    this.municipios = [];
    this.codigosPostales = [];
    this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
    this.itemForm.get('codigoPostalId')!.disable();

    if (!provinciaId) {
      this.itemForm.get('municipioId')!.disable();
      return;
    }

    this.itemForm.get('municipioId')!.enable();
    this.municipioService.getByProvinciaId(provinciaId).subscribe({
      next: list => this.municipios = list,
      error: err => console.error('Error loading municipios', err)
    });
  }

  private loadCodigoPostales(municipioId?: number): void {
    this.codigosPostales = [];
    this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });

    if (!municipioId) {
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }

    this.itemForm.get('codigoPostalId')!.enable();
    this.codigoPostalService.getByMunicipioId(municipioId).subscribe({
      next: list => this.codigosPostales = list,
      error: err => console.error('Error loading codigos postales', err)
    });
  }

  onPaisChange(value: number | null): void {
    const paisId = value ? +value : undefined;
    if (!paisId) {
      // limpiar y deshabilitar hijos
      this.itemForm.patchValue({ provinciaId: null, municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.provincias = [];
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('provinciaId')!.disable();
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadProvincias(paisId);
  }

  onProvinciaChange(value: number | null): void {
    const provinciaId = value ? +value : undefined;
    if (!provinciaId) {
      this.itemForm.patchValue({ municipioId: null, codigoPostalId: null }, { emitEvent: false });
      this.municipios = [];
      this.codigosPostales = [];
      this.itemForm.get('municipioId')!.disable();
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadMunicipios(provinciaId);
  }

  onMunicipioChange(value: number | null): void {
    const municipioId = value ? +value : undefined;
    if (!municipioId) {
      this.itemForm.patchValue({ codigoPostalId: null }, { emitEvent: false });
      this.codigosPostales = [];
      this.itemForm.get('codigoPostalId')!.disable();
      return;
    }
    this.loadCodigoPostales(municipioId);
  }

  private loadItem(): void {
    if (!this.itemId) return;
    this.loading = true;
    this.error = null;

    this.clienteService.getById(this.itemId).subscribe({
      next: item => {
        // Primero parchear los valores no dependientes
        this.itemForm.patchValue({
          codigo: item.codigo,
          descripcion: item.descripcion,
          nif: item.nif,
          iban: item.iban,
          bicoSwift: item.bicoSwift,
          iva: item.iva,
          direccion: item.direccion,
          web: item.web,
          email1: item.email1,
          email2: item.email2,
          telefono1: item.telefono1,
          telefono2: item.telefono2,
          fax1: item.fax1,
          fax2: item.fax2,
          cancelado: item.cancelado
        }, { emitEvent: false });

        // Paises ya cargados en ngOnInit, ahora cargar cascada en secuencia para mantener selección
        if (item.paisId) {
          // habilitar provincia y cargar lista
          this.itemForm.get('provinciaId')!.enable();
          this.provinciaService.getByPaisId(item.paisId).subscribe({
            next: provinces => {
              this.provincias = provinces;
              if (item.provinciaId) {
                this.itemForm.patchValue({ paisId: item.paisId, provinciaId: item.provinciaId }, { emitEvent: false });

                // cargar municipios
                this.itemForm.get('municipioId')!.enable();
                this.municipioService.getByProvinciaId(item.provinciaId).subscribe({
                  next: municipios => {
                    this.municipios = municipios;
                    if (item.municipioId) {
                      this.itemForm.patchValue({ municipioId: item.municipioId }, { emitEvent: false });

                      // cargar codigos postales
                      this.itemForm.get('codigoPostalId')!.enable();
                      this.codigoPostalService.getByMunicipioId(item.municipioId).subscribe({
                        next: cps => {
                          this.codigosPostales = cps;
                          if (item.codigoPostalId) {
                            this.itemForm.patchValue({ codigoPostalId: item.codigoPostalId }, { emitEvent: false });
                          }
                          this.loading = false;
                        },
                        error: err => {
                          console.error('Error loading cp', err);
                          this.loading = false;
                        }
                      });
                    } else {
                      // no hay municipio seleccionado
                      this.loading = false;
                    }
                  },
                  error: err => {
                    console.error('Error loading municipios', err);
                    this.loading = false;
                  }
                });
              } else {
                // no hay provincia seleccionada
                this.itemForm.patchValue({ paisId: item.paisId }, { emitEvent: false });
                this.loading = false;
              }
            },
            error: err => {
              console.error('Error loading provincias', err);
              this.loading = false;
            }
          });
        } else {
          // sin país seleccionado; simplemente setear paisId si viene
          if (item.paisId) this.itemForm.patchValue({ paisId: item.paisId }, { emitEvent: false });
          this.loading = false;
        }
      },
      error: err => {
        this.error = err.message || 'Error al cargar el cliente';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = null;

    const formData: ClienteDTO = {
      ...this.itemForm.value,
      id: this.itemId
    };

    const request = this.isEditMode
      ? this.clienteService.update(this.itemId!, formData)
      : this.clienteService.create(formData);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/nomencladores/clientes']);
      },
      error: err => {
        this.error = err.message || 'Error al guardar el cliente';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/nomencladores/clientes']);
  }
}
