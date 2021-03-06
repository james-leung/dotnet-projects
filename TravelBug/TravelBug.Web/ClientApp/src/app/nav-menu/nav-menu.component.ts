import { Component, OnInit, NgZone } from "@angular/core";
import { AccountService } from "../services/account.service";
import { Subscription } from "rxjs";
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  NavigationEnd,
  Router,
  RoutesRecognized,
} from "@angular/router";
import { FetchDataService } from "../services/fetch-data.service";
import { filter } from "rxjs/operators";
import { LoadingService } from "../services/loading.service";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  writingBlog = false;
  isLoggedIn: boolean;
  isLoading = false;
  loginSub: Subscription;
  routerSub: Subscription;
  loadingSub: Subscription;
  currentUsername: string;

  constructor(
    private accountService: AccountService,
    private fetchDataService: FetchDataService,
    private router: Router,
    private activedRoute: ActivatedRoute,
    private loadingService: LoadingService,
    private ngZone: NgZone
  ) {
    this.isLoggedIn = this.accountService.hasToken;
  }

  ngOnInit() {
    this.loadingSub = this.loadingService.loading.subscribe(
      (l) => (this.isLoading = l)
    );

    this.isLoggedIn = this.accountService.hasToken;
    this.currentUsername = window.localStorage.getItem("travelBug:Username");

    this.routerSub = this.router.events
      .pipe(filter((res) => res instanceof NavigationEnd))
      .subscribe((res: NavigationEnd) => {
        // Check whether we're writing / editing a blog
        // If yes, disable "new blog" button on top right
        if (["new-blog", "edit-blog"].includes(res.url.split("/")[1])) {
          console.log("Disable top right button");
          this.writingBlog = true;
        } else {
          this.writingBlog = false;
        }
      });

    this.loginSub = this.accountService.loginStatus.subscribe((loginStatus) => {
      this.ngZone.run(() => {
        console.log("Logged in");
        this.isLoggedIn = loginStatus;
        this.currentUsername = window.localStorage.getItem(
          "travelBug:Username"
        );
      });
    });
  }

  triggerLoadingBar() {
    // loading
  }

  loadAndRedirect(route: string) {
    this.fetchDataService.fetchData(route);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnDestroy() {
    this.loginSub.unsubscribe();
    this.loadingSub.unsubscribe();
  }
}
