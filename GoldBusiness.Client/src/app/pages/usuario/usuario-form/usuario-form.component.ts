import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UsuarioDTO, UsuarioService } from '../../../services/usuario.service';

@Component({
  selector: 'app-usuario-form',
  templateUrl: './usuario-form.component.html',
  styleUrls: ['./usuario-form.component.css']
})
export class UsuarioFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  itemId?: string;
  loading = false;
  saving = false;
  error: string | null = null;

  availableRoles: string[] = [];
  availablePermissions: string[] = [];

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      userName: ['', [Validators.required, Validators.maxLength(50)]],
      useEmailAsUsername: [true],
      fullName: ['', [Validators.required, Validators.maxLength(120)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(120)]],
      authProvider: ['Local', [Validators.required]],
      password: ['', [Validators.minLength(8)]],
      confirmPassword: [''],
      roles: [[] as string[], [Validators.required]],
      permissions: [['ERP:FullAccess'] as string[], [Validators.required]],
      accessLevelsText: ['*'],
      isActive: [true]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.usuarioService.getRoles().subscribe({
      next: (roles) => {
        this.availableRoles = roles;
        if (!this.isEditMode && roles.length > 0 && (this.form.get('roles')?.value as string[]).length === 0) {
          this.form.get('roles')?.setValue([roles[0]]);
        }
      },
      error: () => {
        this.availableRoles = ['ADMINISTRADOR', 'DESARROLLADOR', 'ECONOMICO', 'CONTADOR'];
      }
    });

    this.usuarioService.getPermissions().subscribe({
      next: (permissions) => {
        this.availablePermissions = permissions;
        if (!this.isEditMode && permissions.length > 0 && (this.form.get('permissions')?.value as string[]).length === 0) {
          this.form.get('permissions')?.setValue([permissions[0]]);
        }
      },
      error: () => {
        this.availablePermissions = ['ERP:FullAccess', 'ERP:AdminAccess', 'ERP:FinanceAccess', 'ERP:AccountingAccess'];
      }
    });

    this.setupUserNameMode();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.itemId = params['id'];
        this.isEditMode = true;
        this.applyPasswordValidators(this.form.get('authProvider')?.value);
        this.loadItem();
      } else {
        this.applyPasswordValidators(this.form.get('authProvider')?.value);
      }
    });

    this.form.get('authProvider')?.valueChanges.subscribe((provider: string) => {
      this.applyPasswordValidators(provider);
    });
  }

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value as string;
    const confirmPassword = control.get('confirmPassword')?.value as string;

    if (!password && !confirmPassword) {
      return null;
    }

    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  loadItem(): void {
    if (!this.itemId) return;

    this.loading = true;
    this.usuarioService.getById(this.itemId).subscribe({
      next: (item) => {
        this.form.patchValue({
          userName: item.userName,
          useEmailAsUsername: item.userName?.toLowerCase() === item.email?.toLowerCase(),
          fullName: item.fullName,
          email: item.email,
          authProvider: item.authProvider ?? 'Local',
          roles: item.roles?.length ? item.roles : [],
          permissions: item.permissions?.length ? item.permissions : [],
          accessLevelsText: (item.accessLevels ?? []).join(', '),
          isActive: item.isActive,
          password: '',
          confirmPassword: ''
        });

        this.applyUserNameMode(this.form.get('useEmailAsUsername')?.value === true);
        this.loading = false;
      },
      error: (err: Error) => {
        this.error = err.message || 'Error al cargar usuario';
        this.loading = false;
      }
    });
  }

  onRoleChange(role: string, checked: boolean): void {
    const current = [...(this.form.get('roles')?.value as string[])];

    if (checked) {
      if (!current.includes(role)) {
        current.push(role);
      }
    } else {
      const index = current.indexOf(role);
      if (index >= 0) {
        current.splice(index, 1);
      }
    }

    this.form.get('roles')?.setValue(current);
    this.form.get('roles')?.markAsTouched();
  }

  hasRole(role: string): boolean {
    const current = this.form.get('roles')?.value as string[];
    return current.includes(role);
  }

  onPermissionChange(permission: string, checked: boolean): void {
    const current = [...(this.form.get('permissions')?.value as string[])];

    if (checked) {
      if (!current.includes(permission)) {
        current.push(permission);
      }
    } else {
      const index = current.indexOf(permission);
      if (index >= 0) {
        current.splice(index, 1);
      }
    }

    this.form.get('permissions')?.setValue(current);
    this.form.get('permissions')?.markAsTouched();
  }

  hasPermission(permission: string): boolean {
    const current = this.form.get('permissions')?.value as string[];
    return current.includes(permission);
  }

  get isLocalProvider(): boolean {
    return this.form.get('authProvider')?.value === 'Local';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = null;

    const value = this.form.getRawValue();
    const payload: UsuarioDTO = {
      id: this.itemId,
      userName: value.userName,
      fullName: value.fullName,
      email: value.email,
      roles: value.roles,
      permissions: value.permissions,
      accessLevels: this.parseCsv(value.accessLevelsText, true),
      authProvider: value.authProvider,
      isActive: value.isActive,
      password: value.password || undefined
    };

    const request = this.isEditMode && this.itemId
      ? this.usuarioService.update(this.itemId, payload)
      : this.usuarioService.create(payload);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/configuracion/usuarios']);
      },
      error: (err: Error) => {
        this.error = err.message || 'Error al guardar usuario';
        this.saving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/configuracion/usuarios']);
  }

  private applyPasswordValidators(provider: string): void {
    const passwordControl = this.form.get('password');
    const confirmControl = this.form.get('confirmPassword');
    if (!passwordControl) return;

    passwordControl.clearValidators();

    const validators = [Validators.minLength(8)];
    if (!this.isEditMode && provider === 'Local') {
      validators.unshift(Validators.required);
    }

    passwordControl.setValidators(validators);
    passwordControl.updateValueAndValidity();

    if (provider !== 'Local') {
      passwordControl.setValue('', { emitEvent: false });
      confirmControl?.setValue('', { emitEvent: false });
    }
  }

  private setupUserNameMode(): void {
    this.form.get('useEmailAsUsername')?.valueChanges.subscribe((useEmail: boolean) => {
      this.applyUserNameMode(useEmail);
    });

    this.form.get('email')?.valueChanges.subscribe((email: string) => {
      if (this.form.get('useEmailAsUsername')?.value === true) {
        this.form.get('userName')?.setValue((email ?? '').trim(), { emitEvent: false });
      }
    });

    this.applyUserNameMode(this.form.get('useEmailAsUsername')?.value === true);
  }

  private applyUserNameMode(useEmail: boolean): void {
    const userNameControl = this.form.get('userName');
    const email = (this.form.get('email')?.value ?? '').trim();
    if (!userNameControl) return;

    if (useEmail) {
      userNameControl.setValue(email, { emitEvent: false });
      userNameControl.disable({ emitEvent: false });
    } else {
      userNameControl.enable({ emitEvent: false });
    }

    userNameControl.updateValueAndValidity({ emitEvent: false });
  }

  private parseCsv(raw: string, uppercase: boolean = false): string[] {
    const parsed = (raw || '')
      .split(',')
      .map((value: string) => value.trim())
      .filter((value: string) => !!value);

    const normalized = uppercase ? parsed.map((value: string) => value.toUpperCase()) : parsed;
    return Array.from(new Set(normalized));
  }
}
