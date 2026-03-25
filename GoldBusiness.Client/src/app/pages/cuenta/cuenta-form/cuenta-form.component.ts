import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { TranslationService } from '../../../services/translation.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-cuenta-form',
  templateUrl: './cuenta-form.component.html',
  styleUrls: ['./cuenta-form.component.css']
})
export class CuentaFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isEditMode = false;
  cuentaId: number | null = null;
  loading = false;
  error: string | null = null;

  allSubGrupos: SubGrupoCuentaDTO[] = [];
  filteredSubGrupos: SubGrupoCuentaDTO[] = [];
  loadingSubGrupos = true;

  gruposCuenta: GrupoCuentaDTO[] = [];
  loadingGrupos = true;

  // lista de negocios (SystemConfiguration)
  systemConfigurations: SystemConfigurationDTO[] = [];
  loadingSystemConfigurations = true;

  selectedSubGrupoCodigo: string = '';
  codigoCompleto: string = '';

  private languageSubscription?: Subscription;

  constructor(
    private fb: FormBuilder,
    private cuentaService: CuentaService,
    private subGrupoCuentaService: SubGrupoCuentaService,
    private grupoCuentaService: GrupoCuentaService,
    private systemConfigurationService: SystemConfigurationService,
    private router: Router,
    private route: ActivatedRoute,
    private translate: TranslationService,
    private languageService: LanguageService
  ) {
    this.form = this.fb.group({
      codigo: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern(/^\d{8}$/)]],
      codigoUsuario: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d{3}$/), Validators.minLength(3), Validators.maxLength(3)]],
      grupoCuentaId: [null, Validators.required],
      subGrupoCuentaId: [{ value: null, disabled: true }, Validators.required],
      descripcion: ['', [Validators.required, Validators.maxLength(256)]],
      systemConfigurationId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.setupFormSubscriptions();
    this.loadGruposCuenta();
    this.loadAllSubGrupos();
    this.loadSystemConfigurations();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.cuentaId = +id;
    }

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 CuentaForm: Idioma cambiado, recargando datos...');
        this.loadGruposCuenta();
        this.loadAllSubGrupos();
        this.loadSystemConfigurations();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  private setupFormSubscriptions(): void {
    this.form.get('grupoCuentaId')?.valueChanges.subscribe(grupoId => {
      this.onGrupoCuentaChange(grupoId);
    });

    this.form.get('subGrupoCuentaId')?.valueChanges.subscribe(subId => {
      this.onSubGrupoChange(subId);
    });

    this.form.get('codigoUsuario')?.valueChanges.subscribe(() => {
      this.updateCodigoCompleto();
    });

    this.form.get('descripcion')?.valueChanges.subscribe(value => {
      if (value && typeof value === 'string' && value !== value.toUpperCase()) {
        this.form.get('descripcion')?.setValue(value.toUpperCase(), { emitEvent: false });
      }
    });
  }

  loadGruposCuenta(): void {
    this.loadingGrupos = true;
    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.gruposCuenta = data.filter(g => !g.cancelado);
        this.loadingGrupos = false;
        this.checkInitialLoad();
      },
      error: (err: any) => {
        console.error('Error loading grupos:', err);
        this.error = 'Error al cargar grupos de cuenta';
        this.loadingGrupos = false;
      }
    });
  }

  loadAllSubGrupos(): void {
    this.loadingSubGrupos = true;
    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.allSubGrupos = data.filter(s => !s.cancelado);
        this.filteredSubGrupos = [];
        this.loadingSubGrupos = false;
        this.checkInitialLoad();
      },
      error: (err: any) => {
        console.error('Error al cargar subgrupos:', err);
        this.error = 'Error al cargar subgrupos de cuenta';
        this.loadingSubGrupos = false;
      }
    });
  }

  loadSystemConfigurations(): void {
    this.loadingSystemConfigurations = true;
    this.systemConfigurationService.getAll().subscribe({
      next: (data) => {
        this.systemConfigurations = data.filter(sc => (sc.estaVigente ?? true));
        this.loadingSystemConfigurations = false;
        if (this.systemConfigurations.length > 0) {
          this.form.get('systemConfigurationId')?.enable({ emitEvent: false });
        } else {
          this.form.get('systemConfigurationId')?.enable({ emitEvent: false });
        }
      },
      error: (err: any) => {
        console.error('Error al cargar configuraciones de sistema (negocios):', err);
        this.loadingSystemConfigurations = false;
        this.error = 'Error al cargar negocios';
      }
    });
  }

  private checkInitialLoad(): void {
    if (this.isEditMode && this.cuentaId && !this.loadingGrupos && !this.loadingSubGrupos) {
      this.loadCuenta();
    }
  }

  onGrupoCuentaChange(grupoId: number | null): void {
    if (!grupoId) {
      this.filteredSubGrupos = [];
      this.form.get('subGrupoCuentaId')?.setValue(null, { emitEvent: false });
      this.form.get('subGrupoCuentaId')?.disable();
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('', { emitEvent: false });
      this.selectedSubGrupoCodigo = '';
      this.updateCodigoCompleto();
      return;
    }

    const gid = Number(grupoId);
    this.filteredSubGrupos = this.allSubGrupos.filter(s => Number(s.grupoCuentaId) === gid);
    this.form.get('subGrupoCuentaId')?.setValue(null);
    this.form.get('subGrupoCuentaId')?.enable();
    this.form.get('codigoUsuario')?.disable();
    this.form.get('codigoUsuario')?.setValue('', { emitEvent: false });
    this.selectedSubGrupoCodigo = '';
    this.updateCodigoCompleto();
  }

  onSubGrupoChange(subGrupoId: number | null): void {
    if (!subGrupoId) {
      this.selectedSubGrupoCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('', { emitEvent: false });
      this.updateCodigoCompleto();
      return;
    }

    const subIdNumber = Number(subGrupoId);
    const subSeleccionado = this.allSubGrupos.find(s => Number(s.id) === subIdNumber);

    if (!subSeleccionado) {
      this.selectedSubGrupoCodigo = '';
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('', { emitEvent: false });
      this.updateCodigoCompleto();
      return;
    }

    this.selectedSubGrupoCodigo = subSeleccionado.codigo || '';
    if (this.selectedSubGrupoCodigo) {
      this.form.get('codigoUsuario')?.enable();
    } else {
      this.form.get('codigoUsuario')?.disable();
      this.form.get('codigoUsuario')?.setValue('', { emitEvent: false });
    }

    this.updateCodigoCompleto();
  }

  updateCodigoCompleto(): void {
    const codigoUsuario = this.form.get('codigoUsuario')?.value || '';

    if (this.selectedSubGrupoCodigo && codigoUsuario && codigoUsuario.length === 3) {
      this.codigoCompleto = this.selectedSubGrupoCodigo + codigoUsuario;
      // mostrar siempre el código completo (even when in-progress), mantener control sincronizado
      this.form.get('codigo')?.setValue(this.codigoCompleto, { emitEvent: false });
    } else {
      // mostrar prefijo + underscores si existe prefijo, o cadena vacía si no
      this.codigoCompleto = this.selectedSubGrupoCodigo ? `${this.selectedSubGrupoCodigo}___` : '';
      // sincronizar control para que el campo muestre el texto parcial y se mantenga alineado
      this.form.get('codigo')?.setValue(this.codigoCompleto, { emitEvent: false });
    }
  }

  loadCuenta(): void {
    if (!this.cuentaId) return;

    this.loading = true;
    this.cuentaService.getById(this.cuentaId).subscribe({
      next: (data) => {
        const codigoSubGrupo = data.codigo.substring(0, 5);
        const codigoUsuario = data.codigo.substring(5, 8);

        const subgrupo = this.allSubGrupos.find(s => s.codigo === codigoSubGrupo);

        if (subgrupo) {
          this.form.get('grupoCuentaId')?.setValue(subgrupo.grupoCuentaId, { emitEvent: false });
          this.filteredSubGrupos = this.allSubGrupos.filter(s => Number(s.grupoCuentaId) === Number(subgrupo.grupoCuentaId));
          this.form.get('subGrupoCuentaId')?.enable({ emitEvent: false });
          this.form.get('subGrupoCuentaId')?.setValue(subgrupo.id, { emitEvent: false });
          this.selectedSubGrupoCodigo = subgrupo.codigo || '';
        } else {
          this.form.get('subGrupoCuentaId')?.setValue(null, { emitEvent: false });
          this.selectedSubGrupoCodigo = '';
        }

        this.form.get('codigoUsuario')?.setValue(codigoUsuario, { emitEvent: false });
        this.form.get('descripcion')?.setValue(data.descripcion, { emitEvent: false });
        this.form.get('systemConfigurationId')?.setValue(data.systemConfigurationId, { emitEvent: false });

        this.updateCodigoCompleto();

        if (this.isEditMode) {
          this.form.get('grupoCuentaId')?.disable({ emitEvent: false });
          this.form.get('subGrupoCuentaId')?.disable({ emitEvent: false });
          this.form.get('codigoUsuario')?.disable({ emitEvent: false });
          this.form.get('codigo')?.disable({ emitEvent: false });
          // Mantener comportamiento coherente con otros formularios: dejar el select "Negocio" inactivo en edición
          this.form.get('systemConfigurationId')?.disable({ emitEvent: false });
        }

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error al cargar la cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSubmit(): void {
    const codigoUsuario = this.form.get('codigoUsuario')?.value;
    if (this.form.invalid || !this.codigoCompleto || (!codigoUsuario && !this.isEditMode) || (codigoUsuario && codigoUsuario.length !== 3)) {
      this.form.markAllAsTouched();

      if ((!codigoUsuario && !this.isEditMode) || (codigoUsuario && codigoUsuario.length !== 3)) {
        this.form.get('codigoUsuario')?.markAsTouched();
      }
      return;
    }

    this.loading = true;
    this.error = null;

    const raw = this.form.getRawValue();
    const dto: CuentaDTO = {
      ...raw,
      codigo: this.codigoCompleto,
      subGrupoCuentaId: raw.subGrupoCuentaId,
      systemConfigurationId: raw.systemConfigurationId
    };

    if (this.isEditMode) {
      this.cuentaService.update(this.cuentaId!, dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/cuenta']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar la cuenta';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    } else {
      this.cuentaService.create(dto).subscribe({
        next: () => {
          this.router.navigate(['/nomencladores/cuenta']);
        },
        error: (err: any) => {
          this.error = 'Error al guardar la cuenta';
          this.loading = false;
          console.error('Error:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/nomencladores/cuenta']);
  }

  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return this.translate.translate('validation.required');
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return this.translate.translate('validation.maxLength', [maxLength]);
    }
    if (control?.hasError('pattern') || control?.hasError('minlength')) {
      if (fieldName === 'codigoUsuario') {
        return this.translate.translate('validation.codigo3Digitos');
      }
      return this.translate.translate('validation.codigo8Digitos');
    }
    return '';
  }
}
